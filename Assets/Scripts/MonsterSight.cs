using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSight : MonoBehaviour {

    private readonly int MAX_ANGLE = 80;
    private Transform player;
    private Transform monster;
    private RaycastHit hit;
    private Ray ray;
    public float sightMultiplier;
    MonsterBehavior behavior;
    private Vector3 targetDir;
    private float movingPlayerBonus;
    private float timeOfDayMultiplier;
    private float sightDistance;
    private float distance;

    void Start ()
    {        
        monster = transform;
        behavior = monster.parent.parent.GetComponent<MonsterBehavior>();
        player = behavior.player;        
    }
	
	void Update ()
    {
        if (Player.IsStationary)
            movingPlayerBonus = 1;
        else movingPlayerBonus = 2;

        //timeOfDayMultiplier = jokuobjektijossain.timeofday.value
        CheckForPlayer();        
    }

    private void CheckForPlayer()
    {
        targetDir = player.position - monster.position;
        ray = new Ray(monster.position, targetDir);
        distance = Vector3.Distance(monster.position, player.position);
        sightDistance = sightMultiplier * movingPlayerBonus;

        // jos säde osuu pelaajaan && pelaaja on hirviön edessä && etäisyys pelaajaan on riittävän pieni
        if (Vector3.Angle(monster.forward, targetDir) <= MAX_ANGLE &&
            Physics.Raycast(ray, out hit) &&
            hit.collider.tag.Equals("Player") &&
            distance < sightDistance)
        {
            Monster.CanSeePlayer = true;
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
