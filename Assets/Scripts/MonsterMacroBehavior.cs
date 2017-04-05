using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMacroBehavior : MonoBehaviour {

    public Terrain terrain;
    //public int waterLevel;
    public float tipDelay;

    private MonsterBehavior monsterBehavior;      
    private int seed;

	// Use this for initialization
	void Start ()
    {
        seed = 0;
        Monster.LastDetectedPlayerTimer = 0;
        monsterBehavior = GetComponent<MonsterBehavior>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Monster.LastDetectedPlayerTimer += Time.deltaTime;
        if(Monster.LastDetectedPlayerTimer > tipDelay && Monster.CurrentState == Monster.MonsterState.Idle || Input.GetKey(KeyCode.O))
        {
            TipMonster();
            Monster.LastDetectedPlayerTimer = 0;
        }
    }

    public void TipMonster()
    {
        monsterBehavior.ResetSurvey();
        Vector3 newVector = BuildPointOfInterestNearPlayer(10,15);        
        //Monster.OnRightTrail = false;
        Monster.LastKnownPlayerPosition = newVector;
        Monster.OriginalPos = newVector;
        Monster.CurrentState = Monster.MonsterState.Investigate;        
    }

    public Vector3 BuildPointOfInterestNearPlayer(int minDistance, int maxDistance)
    {
        Vector2 playerPosVector = new Vector2(monsterBehavior.player.position.x, monsterBehavior.player.position.z);
        Vector2 searchVector;
        Vector3 result;
        float tempHeight;

        seed = Random.Range(minDistance, maxDistance);
        searchVector = playerPosVector + (Random.insideUnitCircle * seed);
        tempHeight = terrain.SampleHeight(searchVector);    
        /*while(tempHeight < waterLevel)
        {
            seed = Random.Range(minDistance, maxDistance);
            searchVector = playerPosVector + (Random.insideUnitCircle * seed);
            tempHeight = terrain.SampleHeight(searchVector);
        }*/
        result = new Vector3(searchVector.x, tempHeight, searchVector.y);
        return result;
    }

    public Vector3 BuildPointOfInterest(int minDistance, int maxDistance)
    {
        Vector2 playerPosVector = new Vector2(Monster.LastKnownPlayerPosition.x, Monster.LastKnownPlayerPosition.z);
        Vector2 searchVector;
        Vector3 result;
        float tempHeight;

        seed = Random.Range(minDistance, maxDistance);
        searchVector = playerPosVector + (Random.insideUnitCircle * seed);
        tempHeight = terrain.SampleHeight(searchVector);
        /*while (tempHeight < waterLevel)
        {
            seed = Random.Range(minDistance, maxDistance);
            searchVector = playerPosVector + (Random.insideUnitCircle * seed);
            tempHeight = terrain.SampleHeight(searchVector);
        }*/
        result = new Vector3(searchVector.x, tempHeight, searchVector.y);
        return result;
    }
}
