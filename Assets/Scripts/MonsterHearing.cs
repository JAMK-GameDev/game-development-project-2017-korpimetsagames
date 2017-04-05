using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHearing : MonoBehaviour {

    private Transform monster;
    private Transform player;
    private MonsterBehavior behavior;
    public double hearingMultiplier;
	// Use this for initialization
	void Start ()
    {
        monster = transform.parent.parent.transform;
        behavior = monster.GetComponent<MonsterBehavior>();
        player = behavior.player;    
	}
	
	// Update is called once per frame
	void Update ()
    {
        // jos pelaajan ja monsterin välimatka riittävän pieni && monster ei valmiiksi näe pelaajaa
        if (Vector3.Distance(monster.position, player.position) < (hearingMultiplier*(int)Player.CurrentState) && Monster.CurrentState != Monster.MonsterState.Chase)
        {            
            Monster.LearnPlayerPosition(player.position);
            behavior.ResetSurvey();
            //Monster.OnRightTrail = true;
            Monster.LastDetectedPlayerTimer = 0;
            Monster.CurrentState = Monster.MonsterState.Investigate;
        }
    }
}
