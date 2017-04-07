using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Author
// Prometheus

public class MonsterBehavior : MonoBehaviour {

    public Transform player;
    public int walkSpeed;
    public int runSpeed;
    private float surveyTimeLimit;   // kuinka kauan monsteri kääntyilee korkeintaan
    private float turnSpeed;        // kääntymisnopeus
    private float surveyTimer;      // kuinka kauan monsteri on kääntyillyt paikallaan
    private float surveyStateChangeTimer;
    private float surveyStateChangeTimeLimit;
    private float seed;
    private int totalSearches;
    private int surveyCount;
    private bool wasTurning;
    private bool searchLocationsAdded;
    private Transform body;
    private Transform monster;
    private NavMeshAgent navMeshAgent;      
    private List<Vector3> pointsOfInterest;
    private MonsterMacroBehavior macroBehavior;
    private bool waitedForDistanceUpdate;
    private bool wait;
    
    public enum SurveyState
    {
        LookLeft,
        LookRight,
        LookForward
    }

    private SurveyState surveyState;    
    
    void Start()
    {
        totalSearches = Random.Range(3, 6);
        surveyTimeLimit = Random.Range(4, 7);
        surveyTimer = 0;
        surveyCount = 0;
        surveyStateChangeTimer = 0;
        surveyStateChangeTimeLimit = 1;
        turnSpeed = Random.Range(50, 200);
        searchLocationsAdded = false;
        wait = true;
        monster = transform;        
        surveyState = SurveyState.LookForward;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.acceleration = 10;
        navMeshAgent.angularSpeed = 999;
        navMeshAgent.speed = walkSpeed;
        macroBehavior = GetComponent<MonsterMacroBehavior>();
        body = monster.FindChild("Body");
        Monster.CurrentState = Monster.MonsterState.Idle;
        Monster.CanSeePlayer = false;
        Monster.OriginalPos = monster.position;
        Monster.LastKnownPlayerPosition = player.position;
    }
    
    void Update()
    {
        //print("Pelaaja: " + player.position + ", lastKnownPos: " + Monster.LastKnownPlayerPosition + ", monster: " + monster.position + "MonsterState: " + Monster.CurrentState);
        switch (Monster.CurrentState)
        {
            case Monster.MonsterState.Chase: Chase(); break;
            case Monster.MonsterState.Idle: Idle(); break;
            case Monster.MonsterState.Investigate: Investigate(); break;
            case Monster.MonsterState.Survey: Survey(); break;
            case Monster.MonsterState.Search: Search(); break;
            default: return;
        }
    }


    public void ResetSurvey()
    {
        surveyStateChangeTimer = 0;
        surveyTimer = 0;
        surveyCount = 0;
        searchLocationsAdded = false;
        totalSearches = Random.Range(3, 6);
        surveyTimeLimit = Random.Range(4, 7);
    }

    private void Chase()
    {
        //print("Chasing");
        navMeshAgent.speed = runSpeed;
        Monster.OriginalPos = monster.position;

        if (!Monster.CanSeePlayer)
        {
            Monster.CurrentState = Monster.MonsterState.Investigate;
            return;
        }

        body.GetComponent<Renderer>().material.color = Color.red;
        navMeshAgent.destination = Monster.LastKnownPlayerPosition;
    }

    private void Investigate()
    {
        print("Investigating, my position: " + monster.position + ", target position: " + Monster.LastKnownPlayerPosition +". Stopping distance: " + navMeshAgent.stoppingDistance + ", distance to target: " + Vector3.Distance(monster.position, Monster.LastKnownPlayerPosition));
        // välillä monsteri jää investigateen jumiin, kun sille annettuun sijaintiin ei saa laskettua reittiä
        body.GetComponent<Renderer>().material.color = Color.yellow;
        navMeshAgent.destination = Monster.LastKnownPlayerPosition;
        
        //print("Monster pos: " + monster.position + ", destination pos: " + navMeshAgent.destination +". Stopping distance: " + navMeshAgent.stoppingDistance + ", remaining distance: " + navMeshAgent.remainingDistance + ", pathpending: " + navMeshAgent.pathPending + ", hasPath: " + navMeshAgent.hasPath + ", velocity: " + navMeshAgent.velocity.sqrMagnitude);
        if(PathComplete())
        {
            Monster.CurrentState = Monster.MonsterState.Survey;
            return;         
        }
        else if (navMeshAgent.path.status == NavMeshPathStatus.PathInvalid)
        {
            navMeshAgent.SetDestination(Monster.LastKnownPlayerPosition);
        }

        // täytyy tietää tutkitaanko sijaintia joka on randomoitu vai sijaintia jossa pelaaja on havaittu
        // jos pathfinding ei toimi haluttuun lokaatioon, niin täytyy laskea uusi reitti. Ongelma: asynkroninen navmeshagent ei ehdi laskea uutta reittiä ennen tätä if-lausetta. Se jää jumiin.
       /* if (!navMeshAgent.hasPath && !Monster.OnRightTrail)
        {
            navMeshAgent.ResetPath();
            navMeshAgent.SetDestination(BuildPointOfInterest());
            navMeshAgent.Resume();
        }
        else if(!navMeshAgent.hasPath && Monster.OnRightTrail)
        {
            navMeshAgent.ResetPath();
            navMeshAgent.SetDestination(Monster.LastKnownPlayerPosition);
            navMeshAgent.Resume();
        }*/
    }

    private void Survey()
    {
        //print("Surveying");
        navMeshAgent.speed = walkSpeed;

        if (surveyCount == totalSearches)
        {
            Monster.CurrentState = Monster.MonsterState.Idle;
            ResetSurvey();
            return;
        }

        // jos on katseltu ympäriinsä tässä pisteessä riittävän kauan 
        // wasturning siksi ettei monsteri sinkoa muualle kesken kääntymisen (kuuluu olla true)
        if (surveyTimer > surveyTimeLimit && wasTurning)
        {
            surveyTimer = 0;
            surveyCount++;
            Monster.CurrentState = Monster.MonsterState.Search;
            return;
        }
        
        // TODO: innostusasteet jotka määrittää kääntymisen nopeutta ja tiheyttä

        if(surveyStateChangeTimer > surveyStateChangeTimeLimit)
        {                       
            turnSpeed = Random.Range(50, 200);
            surveyStateChangeTimeLimit = Random.Range(200, 400) / (turnSpeed*2); // jos käännytään nopeasti, ei käännytä kauan
            surveyStateChangeTimer = 0;            

            if(wasTurning)
            {
                surveyState = SurveyState.LookForward;
            }
            else
            {
                // randomoidaan suunta
                seed = Random.Range(0, 100);
                if (seed >= 50) surveyState = SurveyState.LookLeft;
                else surveyState = SurveyState.LookRight;
            }
        }

        switch (surveyState)
        {
            case SurveyState.LookLeft:
                wasTurning = true;
                monster.Rotate(Vector3.down * Time.deltaTime * turnSpeed);
                break;
            case SurveyState.LookRight:
                wasTurning = true;
                monster.Rotate(-Vector3.down * Time.deltaTime * turnSpeed);
                break;
            case SurveyState.LookForward:
                wasTurning = false;
                break;
            default: return;
        }

        surveyStateChangeTimer += Time.deltaTime;
        surveyTimer += Time.deltaTime;
        body.GetComponent<Renderer>().material.color = Color.cyan;
    }

    private void Search()
    {
        //print("Searching");
        body.GetComponent<Renderer>().material.color = Color.magenta;
        
        if(!searchLocationsAdded)
        {
            pointsOfInterest = new List<Vector3>();            

            // randomoidaan lokaatioita xz-akseleilla ja haetaan niille oikeat korkeudet terrainista
            for (int i = 0; i <= totalSearches; i++)
            {                
                pointsOfInterest.Add(macroBehavior.BuildPointOfInterest(5,12));
            }

            searchLocationsAdded = true;
        }

        navMeshAgent.destination = pointsOfInterest[surveyCount];

        if (PathComplete())
        {
            surveyState = SurveyState.LookForward;
            Monster.CurrentState = Monster.MonsterState.Survey;
            return;
        }
        else if(navMeshAgent.path.status == NavMeshPathStatus.PathInvalid)
        {
            pointsOfInterest[surveyCount] = macroBehavior.BuildPointOfInterest(5, 12);
        }
    }

    private void Idle()
    {
        //print("Idle");
        body.GetComponent<Renderer>().material.color = Color.green;
        navMeshAgent.destination = Monster.OriginalPos;
    }

    private bool PathComplete()
    {
        Vector2 target = new Vector2(Monster.LastKnownPlayerPosition.x, Monster.LastKnownPlayerPosition.z);
        Vector2 monst = new Vector2(monster.position.x, monster.position.z);
        float distance = Vector3.Distance(monst, target);

        // remainingDistance päivittyy vasta yhden framen päästä siitä kun määränpää on asetettu, vaatii ylimääräisen tarkistuksen.
        if (distance <= navMeshAgent.stoppingDistance && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending && navMeshAgent.velocity.sqrMagnitude == 0f)
        {
            return true;
        }
        else return false;
    }
}