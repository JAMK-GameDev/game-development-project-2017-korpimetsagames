using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSight : MonoBehaviour {

    private readonly int MAX_ANGLE = 80;
    private Transform player;
    private Transform monster;
    private RaycastHit hit;
    private Ray ray;
    public double SightDistance;
    MonsterBehavior behavior;

    // Use this for initialization
    void Start ()
    {
        player = GetComponent<MonsterBehavior>().player;
        monster = transform;
        behavior = monster.GetComponent<MonsterBehavior>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 targetDir = player.position - monster.position;
        ray = new Ray(monster.position, targetDir);

        // jos säde osuu johonkin && pelaaja on hirviön edessä && etäisyys pelaajaan on riittävän pieni
        if (Vector3.Angle(monster.forward, targetDir) <= MAX_ANGLE && Physics.Raycast(ray, out hit) &&             
            hit.collider.tag.Equals("Player") &&
            Vector3.Distance(monster.position, player.position) < SightDistance)
        {
            behavior.SeePlayer();
            behavior.LearnPlayerPosition();
            behavior.ResetSurvey();
            Monster.CurrentState = Monster.MonsterState.Chase;
        }
        else
        {
            behavior.LosePlayer();
        }
    }
}
