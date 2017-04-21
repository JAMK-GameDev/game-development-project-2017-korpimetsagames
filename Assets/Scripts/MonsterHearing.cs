using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterHearing : MonoBehaviour {

    private Transform monster;
    private Transform player;
    private MonsterBehavior behavior;
    public float hearingMultiplier;
    private float distance;
    private float hearingDistance;

	void Start ()
    {
        monster = transform.transform;
        behavior = monster.GetComponent<MonsterBehavior>();
        player = behavior.player;    
	}
	
	void Update ()
    {
        distance = Vector3.Distance(monster.position, player.position);
        hearingDistance = (hearingMultiplier * Player.NoiseLevel);

        if (distance < hearingDistance && Monster.CurrentState != Monster.MonsterState.Chase)
        {            
            Monster.LearnPlayerPosition(player.position);
            behavior.ResetSurvey();
            Monster.LastDetectedPlayerTimer = 0;
        }
    }
}

