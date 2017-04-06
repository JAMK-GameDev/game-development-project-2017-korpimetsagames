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
    private Vector3 targetDir;
    private float sightMultiplier;
    private float timeOfDayMultiplier;

    // Use this for initialization
    void Start ()
    {        
        monster = transform;
        behavior = monster.parent.parent.GetComponent<MonsterBehavior>();
        player = behavior.player;        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Player.IsStationary)
            sightMultiplier = 1;
        else sightMultiplier = 2;

        //timeOfDayMultiplier = jokuobjektijossain.timeofday.value
        CheckForPlayer();        
    }

    private void CheckForPlayer()
    {
        targetDir = player.position - monster.position;
        ray = new Ray(monster.position, targetDir);
        
        // jos säde osuu johonkin && pelaaja on hirviön edessä && etäisyys pelaajaan on riittävän pieni
        if (Vector3.Angle(monster.forward, targetDir) <= MAX_ANGLE &&
            Physics.Raycast(ray, out hit) &&
            hit.collider.tag.Equals("Player") &&
            Vector3.Distance(monster.position, player.position) < SightDistance * sightMultiplier)
        {
            Monster.CanSeePlayer = true;
            //Monster.OnRightTrail = true;
            Monster.LastDetectedPlayerTimer = 0;
            Monster.LearnPlayerPosition(player.position);
            behavior.ResetSurvey();
            Monster.CurrentState = Monster.MonsterState.Chase;
        }
        else
        {
            Monster.CanSeePlayer = false;
        }
    }
}
