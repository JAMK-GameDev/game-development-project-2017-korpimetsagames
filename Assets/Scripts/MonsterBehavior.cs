using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Author
// Prometheus

public class MonsterBehavior : MonoBehaviour {

    public Transform player;
    public Terrain terrain;   
    public float surveyTimeLimit;   // kuinka kauan monsteri kääntyilee korkeintaan
    private float turnSpeed;        // kääntymisnopeus
    private float surveyTimer;      // kuinka kauan monsteri on kääntyillyt
    private float surveyStateChangeTimer;
    private float surveyStateChangeTimeLimit;
    private float seed;
    private int totalSearches;
    private int surveyCount;
    private bool wasTurning;
    private bool searchLocationsAdded;
    private bool canSeePlayer;
    private Transform monster;
    private Vector3 originalPos;
    private Vector3 lastKnownPlayerPosition;    
    private NavMeshAgent navMeshAgent;      
    private List<Vector3> lookUpPositions;
    
    public enum MonsterState
    {
        Chase, // näkee pelaajan ja jahtaa
        Investigate, // ei näe pelaajaa, liikkuu sinne missä pelaaja viimeksi havaittiin        
        Survey, // saapunut sinne missä pelaaja viimeksi havaittu, katselee ympärilleen mutta ei liiku
        Search, // liikkuu ja tutkii lähimaastoa
        Idle // ei merkkejä pelaajasta, palaa sijaintiin _mistä_ pelaaja nähtiin
    }

    public enum SurveyState
    {
        LookLeft,
        LookRight,
        LookForward
    }

    private SurveyState surveyState;
    private MonsterState currentState;

    public MonsterState CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }
    
    void Start()
    {
        totalSearches = Random.Range(3, 6);
        surveyTimer = 0;
        surveyCount = 0;
        surveyStateChangeTimer = 0;
        surveyStateChangeTimeLimit = 1;
        turnSpeed = Random.Range(50, 200);
        searchLocationsAdded = false;
        canSeePlayer = false;
        monster = transform;
        currentState = MonsterState.Idle;
        surveyState = SurveyState.LookForward;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.acceleration = 10;
        navMeshAgent.angularSpeed = 999;
        originalPos = monster.position;
    }
    
    void Update()
    {
        //print(navMeshAgent.remainingDistance);
        switch (currentState)
        {
            case MonsterState.Chase: Chase(); break;
            case MonsterState.Idle: Idle(); break;
            case MonsterState.Investigate: Investigate(); break;
            case MonsterState.Survey: Survey(); break;
            case MonsterState.Search: Search(); break;
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
    }

    public void LearnPlayerPosition()
    {
        lastKnownPlayerPosition = player.position;
    }

    public void SeePlayer()
    {
        canSeePlayer = true;
    }

    public void LosePlayer()
    {
        canSeePlayer = false;
    }

    private void Chase()
    {
        originalPos = monster.position;
        if (!canSeePlayer)
        {
            CurrentState = MonsterState.Investigate;
            return;
        }

        //print("Chasing. Last known position: " + lastKnownPlayerPosition + ". Distance to position: " + navMeshAgent.remainingDistance);
        monster.GetComponent<Renderer>().material.color = Color.red;
        navMeshAgent.destination = player.position;
    }

    private void Investigate()
    {
        //print("Investigating. Last known position: " + lastKnownPlayerPosition + ". Distance to position: " + navMeshAgent.remainingDistance);
        monster.GetComponent<Renderer>().material.color = Color.yellow;
        navMeshAgent.destination = lastKnownPlayerPosition;

        if (navMeshAgent.remainingDistance < 1.5)
        {
            CurrentState = MonsterState.Survey;
            return;         
        }
    }

    private void Idle()
    {
        //print("Idle.");
        monster.GetComponent<Renderer>().material.color = Color.green;
        navMeshAgent.destination = originalPos;
    }

    private void Survey()
    {
        /*if(lookUpPositions != null)
        {
            print("Surveying. Count: " + surveyCount + ". " + lookUpPositions[surveyCount]);
        }
        else
        {
            print("Surveying. Count: " + surveyCount);
        }*/
        
        if(surveyCount == totalSearches)
        {
            CurrentState = MonsterState.Idle;
            ResetSurvey();
            return;
        }

        if (surveyTimer > surveyTimeLimit)
        {
            surveyTimer = 0;
            surveyCount++;
            CurrentState = MonsterState.Search;
            return;
        }
        

        if(surveyStateChangeTimer > surveyStateChangeTimeLimit)
        {                       
            turnSpeed = Random.Range(50, 200);
            surveyStateChangeTimeLimit = Random.Range(50, 300) / turnSpeed;
            surveyStateChangeTimer = 0;
            print("turnspeed: " + turnSpeed + ". timelimit: " + surveyStateChangeTimeLimit);

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
        //monster.GetComponent<Renderer>().material.color = Color.cyan;
    }

    private void Search()
    {
        //monster.GetComponent<Renderer>().material.color = Color.magenta;
        
        if(!searchLocationsAdded)
        {
            lookUpPositions = new List<Vector3>();
            Vector2 playerPosVector = new Vector2(lastKnownPlayerPosition.x, lastKnownPlayerPosition.z);
            Vector2 searchVector;
            Vector3 newVector;
            float tempHeight;

            // randomoidaan lokaatioita xz-akseleilla ja haetaan niille oikeat korkeudet terrainista
            for (int i = 0; i <= totalSearches; i++)
            {               
                seed = Random.Range(5, 12);
                searchVector = playerPosVector + (Random.insideUnitCircle * seed);
                tempHeight = terrain.SampleHeight(searchVector);
                newVector = new Vector3(searchVector.x, tempHeight, searchVector.y);
                lookUpPositions.Add(newVector);
            }
            searchLocationsAdded = true;
        }
        
        navMeshAgent.destination = lookUpPositions[surveyCount];

        if (navMeshAgent.remainingDistance < 1.5)
        {
            CurrentState = MonsterState.Survey;
        }
    }
}