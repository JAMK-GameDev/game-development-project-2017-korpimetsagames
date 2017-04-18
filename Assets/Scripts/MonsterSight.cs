using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC.TimeOfDaySystemFree;

public class MonsterSight : MonoBehaviour {

    public TimeOfDayManager manager;
    public float sightMultiplier;

    private readonly int MAX_ANGLE = 80;
    private Transform player;
    private Transform monster;
    private RaycastHit hit;
    private Ray ray;
    private MonsterBehavior behavior;
    private Vector3 targetDir;
    private float movingPlayerBonus;
    private float timeOfDayMultiplier;
    private float sightDistance;
    private float distance;
    private float sneakMultiplier;

    void Start ()
    {        
        monster = transform;
        behavior = monster.GetComponent<MonsterBehavior>();
        player = behavior.player;
    }
	
	void Update ()
    {        
        CheckForPlayer();
    }

    private void CheckForPlayer()
    {
        targetDir = player.position - monster.position;
        ray = new Ray(monster.position, targetDir);
        distance = Vector3.Distance(monster.position, player.position);

        timeOfDayMultiplier = manager.IsDay ? 2 : 1;
        movingPlayerBonus = Player.IsStationary ? (float)0.75 : (float)1.25;
        if(Player.MoveMode == Player.MoveState.Sneak) { sneakMultiplier = (float)0.8; }
        else if (Player.MoveMode == Player.MoveState.Run) { sneakMultiplier = (float)1.2; }
        else { sneakMultiplier = 1; }

        sightDistance = sightMultiplier * movingPlayerBonus * timeOfDayMultiplier * sneakMultiplier;
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

    private float CalculateTimeOfDayMultiplier()
    {
        float amplitude = 12;
        float f = 1 / 24;
        float t = manager.Hour;
        float result = amplitude * Mathf.Sin(2 * Mathf.PI * f * t);
        return result;
    }
}
