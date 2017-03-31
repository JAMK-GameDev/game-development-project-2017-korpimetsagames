using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHearing : MonoBehaviour {

    private Transform monster;
    private Transform player;
    private MonsterBehavior behavior;
    public double HearingDistance;
	// Use this for initialization
	void Start ()
    {
        monster = transform;
        behavior = monster.GetComponent<MonsterBehavior>();
        player = GetComponent<MonsterBehavior>().player;        
	}
	
	// Update is called once per frame
	void Update ()
    {
        // jos pelaajan ja monsterin välimatka riittävän pieni
        if (Vector3.Distance(monster.position, player.position) < HearingDistance)
        {
            behavior.LearnPlayerPosition();
            behavior.CurrentState = MonsterBehavior.MonsterState.Investigate;
        }
    }
}
