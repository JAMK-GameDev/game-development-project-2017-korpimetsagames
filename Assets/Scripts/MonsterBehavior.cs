using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBehavior : MonoBehaviour {

    public Transform player;
    public float turnSpeed;
    public float surveyTimeLimit;
    private float surveyTimer;
    private float seed;
    private bool canSeePlayer;
    private Transform monster;
    private Vector3 originalPos;
    private Vector3 lastKnownPlayerPosition;    
    private NavMeshAgent navMeshAgent;      
    private List<Vector2> lookUpPositions;
    
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
        LookRight
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
        surveyTimer = 0;
        canSeePlayer = false;
        monster = transform;
        currentState = MonsterState.Idle;
        surveyState = SurveyState.LookLeft;
        navMeshAgent = GetComponent<NavMeshAgent>();
        originalPos = monster.position;
    }
    
    void Update()
    {
        switch (currentState)
        {
            case MonsterState.Chase: Chase(); break;
            case MonsterState.Idle: Idle(); break;
            case MonsterState.Investigate: Investigate(); break;
            case MonsterState.Survey: Survey(); break;
            default: return;
        }
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
        if(surveyTimer > surveyTimeLimit)
        {
            surveyTimer = 0;
            CurrentState = MonsterState.Idle;
            return;
        }

        // TODO: Logiikka jolla määritetään mihin suuntaan monsteri katselee, eli surveyState

        switch (surveyState)
        {
            case SurveyState.LookLeft:
                //print("Looking left.");
                // TODO: Logiikka joka kääntää monsteria
                //monster.Rotate(Vector3.left * Time.deltaTime * turnSpeed);
                break;
            case SurveyState.LookRight:
                //print("Looking right.");
                break;
            default: return;
        }
        surveyTimer += Time.deltaTime;
        //print("Surveying.");
        monster.GetComponent<Renderer>().material.color = Color.blue;     
    }

    private void Search()
    {
        lookUpPositions = new List<Vector2>();

        for (int i = 0; i < 5; i++)
        {
            seed = Random.Range(3, 15);
            lookUpPositions.Add(Random.insideUnitCircle * seed);
        }
    }
}
