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
    private Transform monster;
    private NavMeshAgent navMeshAgent;      
    private List<Vector3> pointsOfInterest;
    private MonsterMacroBehavior macroBehavior;
    
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
        monster = transform;
        Monster.CurrentState = Monster.MonsterState.Idle;
        surveyState = SurveyState.LookForward;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.acceleration = 10;
        navMeshAgent.angularSpeed = 999;
        navMeshAgent.speed = walkSpeed;
        Monster.OriginalPos = monster.position;
        Monster.LastKnownPlayerPosition = player.position;
        macroBehavior = GetComponent<MonsterMacroBehavior>();
    }
    
    void Update()
    {        
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
        navMeshAgent.speed = runSpeed;
        Monster.OriginalPos = monster.position;

        if (!Monster.CanSeePlayer)
        {
            Monster.CurrentState = Monster.MonsterState.Investigate;
            return;
        }
        
        monster.GetComponent<Renderer>().material.color = Color.red;
        navMeshAgent.destination = player.position;
    }

    private void Investigate()
    {
        // välillä monsteri jää investigateen jumiin, kun sille annettuun sijaintiin ei saa laskettua reittiä
        monster.GetComponent<Renderer>().material.color = Color.yellow;
        navMeshAgent.destination = Monster.LastKnownPlayerPosition;
        //print(monster.position + " | " + navMeshAgent.destination +". Stopping distance: " + navMeshAgent.stoppingDistance + ", remaining distance: " + navMeshAgent.remainingDistance + ", pathpending: " + navMeshAgent.pathPending + ", hasPath: " + navMeshAgent.hasPath + ", velocity: " + navMeshAgent.velocity.sqrMagnitude);
        if (PathComplete())
        {
            Monster.CurrentState = Monster.MonsterState.Survey;
            return;         
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
        navMeshAgent.speed = walkSpeed;

        if (surveyCount == totalSearches)
        {
            Monster.CurrentState = Monster.MonsterState.Idle;
            ResetSurvey();
            return;
        }
        
       /* if(pointsOfInterest!= null)
        {
            print("Endpoint: " +pointsOfInterest[surveyCount] + ", monster pos: " + monster.position);
        }*/

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
        monster.GetComponent<Renderer>().material.color = Color.cyan;
    }

    private void Search()
    {
        monster.GetComponent<Renderer>().material.color = Color.magenta;
        
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
        else if(!navMeshAgent.hasPath)
        {
            pointsOfInterest[surveyCount] = macroBehavior.BuildPointOfInterest(5, 12);
        }
    }

    private void Idle()
    {
        monster.GetComponent<Renderer>().material.color = Color.green;
        navMeshAgent.destination = Monster.OriginalPos;
    }

    private bool PathComplete()
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending && (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f))
        {
            return true;
        }
        else return false;
    }
}