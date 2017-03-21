using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour {
    public Transform target;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.destination = target.position;
    }
}