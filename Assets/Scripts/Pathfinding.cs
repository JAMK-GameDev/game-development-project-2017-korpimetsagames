using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour {
    public Transform target;
    private RaycastHit hit;
    private Ray ray;
    private Vector3 lastKnownTargetPos;
    private readonly int MAX_ANGLE = 75;
    // Use this for initialization
    void Start()   {    }
	
	// Update is called once per frame
	void Update ()
    {
        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
        
        Vector3 targetDir = target.transform.position - transform.position;
        ray = new Ray(transform.position, targetDir);

        if(Vector3.Angle(transform.forward, targetDir) <= MAX_ANGLE && Physics.Raycast(ray, out hit))
        {
            //print("Found an object: " + hit.transform.name);

            if (hit.collider.tag.Equals("Player"))
            {
                lastKnownTargetPos = target.position;
                navMeshAgent.destination = lastKnownTargetPos;
            }
        }
        else
        {
            //print("I see nothing");
        }
    }
}