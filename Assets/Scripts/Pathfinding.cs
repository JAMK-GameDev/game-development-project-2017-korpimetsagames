using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour {
    public Transform target;
    private RaycastHit hit;
    private Ray ray;
    // Use this for initialization
    void Start()   {    }
	
	// Update is called once per frame
	void Update ()
    {
        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
        Vector3 targetDir = target.transform.position - transform.position;
        ray = new Ray(transform.position, targetDir);
        navMeshAgent.ResetPath();

        if (Physics.Raycast(ray, out hit))
        {
            print("Found an object: " + hit.transform.name);

            if (hit.collider.tag.Equals("Player"))
            {
                navMeshAgent.destination = target.position;
            }
        }
        else
        {
            print("I see nothing");
        }


    }
}