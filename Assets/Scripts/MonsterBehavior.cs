using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBehavior : MonoBehaviour {

    public Transform player;
    private Transform monster;
    private Vector3 lastKnownPlayerPosition;
    private bool canSeePlayer;
    NavMeshAgent navMeshAgent;
    private Vector3 originalPos;

    public enum MonsterState
    {
        Chase,
        Investigate,
        Idle,
        Survey,
        Search,
        Reset
    }

    private MonsterState currentState;

    public MonsterState CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    // Use this for initialization
    void Start()
    {        
        canSeePlayer = false;
        monster = transform;
        currentState = MonsterState.Idle;
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
            case MonsterState.Reset: Reset(); break;
            //case MonsterState.Survey: Survey(); break;
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
        if (Vector3.Distance(player.position, monster.position) < 2)
        {
            CurrentState = MonsterState.Reset;
            return;
        }

        if (!canSeePlayer)
        {
            CurrentState = MonsterState.Investigate;
            return;
        }

        //print("Chasing player at " + player.position);
        monster.GetComponent<Renderer>().material.color = Color.red;
        navMeshAgent.destination = player.position;
    }

    private void Investigate()
    {
        //print("Investigating. Last known position: " + lastKnownPlayerPosition + ". Distance to position: " + navMeshAgent.remainingDistance);
        monster.GetComponent<Renderer>().material.color = Color.yellow;
        navMeshAgent.destination = lastKnownPlayerPosition;

        // jos saavuttiin sinne missä viimeksi havaittiin pelaaja
        if (navMeshAgent.remainingDistance < 1.5)
        {
            CurrentState = MonsterState.Idle;
            return;
          //  CurrentState = MonsterState.Idle;
            /*
            lookUpPositions = new List<Vector2>();

            for (int i = 0; i < 5; i++)
            {
                seed = Random.Range(3, 15);
                lookUpPositions.Add(Random.insideUnitCircle * seed);
            }*/
        }
    }

    private void Idle()
    {
        //print("Idle.");
        monster.GetComponent<Renderer>().material.color = Color.green;
    }

    private void Survey()
    {
        //print("Surveying.");
        monster.GetComponent<Renderer>().material.color = Color.blue;
        navMeshAgent.Stop();

    }

    private void Reset()
    {
        lastKnownPlayerPosition = originalPos;
        monster.transform.position = originalPos;
        CurrentState = MonsterState.Idle;
    }
}
