using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Author
// Prometheus

public class MonsterBehavior : MonoBehaviour {

    public Transform player;
    public Terrain terrain;   
    private float surveyTimeLimit;   // kuinka kauan monsteri kääntyilee korkeintaan
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
    private NavMeshAgent navMeshAgent;      
    private List<Vector3> lookUpPositions;
    

    

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
        canSeePlayer = false;
        monster = transform;
        Monster.CurrentState = Monster.MonsterState.Idle;
        surveyState = SurveyState.LookForward;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.acceleration = 10;
        navMeshAgent.angularSpeed = 999;
        Monster.OriginalPos = monster.position;
    }
    
    void Update()
    {
        //print(navMeshAgent.remainingDistance);
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

    public void LearnPlayerPosition()
    {
        Monster.LastKnownPlayerPosition = player.position;
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
        Monster.OriginalPos = monster.position;
        if (!canSeePlayer)
        {
            Monster.CurrentState = Monster.MonsterState.Investigate;
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
        navMeshAgent.destination = Monster.LastKnownPlayerPosition;

        if (navMeshAgent.remainingDistance < 1.5)
        {
            Monster.CurrentState = Monster.MonsterState.Survey;
            return;         
        }
    }

    private void Idle()
    {
        //print("Idle.");
        monster.GetComponent<Renderer>().material.color = Color.green;
        navMeshAgent.destination = Monster.OriginalPos;
    }

    private void Survey()
    {
        //print("Survey, " + surveyState);
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
            Monster.CurrentState = Monster.MonsterState.Idle;
            ResetSurvey();
            return;
        }

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
        //print("Search, " + surveyState + ", destination: " + navMeshAgent.destination);
        monster.GetComponent<Renderer>().material.color = Color.magenta;
        
        if(!searchLocationsAdded)
        {
            lookUpPositions = new List<Vector3>();
            Vector2 playerPosVector = new Vector2(Monster.LastKnownPlayerPosition.x, Monster.LastKnownPlayerPosition.z);
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
            surveyState = SurveyState.LookForward;
            Monster.CurrentState = Monster.MonsterState.Survey;
        }
    }
}