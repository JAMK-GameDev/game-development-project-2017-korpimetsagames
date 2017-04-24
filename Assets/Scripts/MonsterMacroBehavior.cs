using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMacroBehavior : MonoBehaviour {

    public float tipDelay;
    private MonsterBehavior monsterBehavior;
    private Transform player;
    private Transform monster;
    private int seed;

	// Use this for initialization
	void Start ()
    {
        seed = 0;
        Monster.LastDetectedPlayerTimer = 0;
        monsterBehavior = GetComponent<MonsterBehavior>();
        monster = transform;
        player = monsterBehavior.player;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Monster.LastDetectedPlayerTimer += Time.deltaTime;
        if(Monster.LastDetectedPlayerTimer > tipDelay && Monster.CurrentState == Monster.MonsterState.Idle || Input.GetKey(KeyCode.O) || Vector3.Distance(player.position, monster.position) > 90 && Monster.CurrentState != Monster.MonsterState.Investigate)
        {
            TipMonster();
            Monster.LastDetectedPlayerTimer = 0;
        }
    }

    public void TipMonster()
    {
        monsterBehavior.ResetSurvey();
        Vector3 newVector = BuildPointOfInterestNearPlayer(10,50);   
        Monster.LastKnownPlayerPosition = newVector;
        Monster.OriginalPos = newVector;
        Monster.CurrentState = Monster.MonsterState.Investigate;
    }

    public Vector3 BuildPointOfInterestNearPlayer(int minDistance, int maxDistance)
    {       
        Vector3 searchVector;
        Vector3 result;
        float tempHeight;
        float playerHeight = (float)1.32;

        seed = Random.Range(minDistance, maxDistance);
        searchVector = player.position + (Random.insideUnitSphere * seed);
        tempHeight = Terrain.activeTerrain.SampleHeight(searchVector) + playerHeight;
        result = new Vector3(searchVector.x, tempHeight, searchVector.z);
        return result;
    }

    public Vector3 BuildPointOfInterest(int minDistance, int maxDistance)
    {        
        Vector3 searchVector;
        Vector3 result;
        float tempHeight;
        float playerHeight = (float)1.32;

        seed = Random.Range(minDistance, maxDistance);
        searchVector = Monster.LastKnownPlayerPosition + (Random.insideUnitSphere * seed);
        tempHeight = Terrain.activeTerrain.SampleHeight(searchVector) + playerHeight;
        result = new Vector3(searchVector.x, tempHeight, searchVector.z);
        return result;
    }
}
