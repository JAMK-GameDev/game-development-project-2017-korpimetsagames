using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMacroBehavior : MonoBehaviour {


    private float lastSeenPlayerTimer;
    public float TimeLimit;
    public Transform player;
    public Terrain terrain;
    int seed;
	// Use this for initialization
	void Start ()
    {
        seed = 0;
        lastSeenPlayerTimer = 0;        
	}
	
	// Update is called once per frame
	void Update ()
    {
        lastSeenPlayerTimer += Time.deltaTime;
        if(lastSeenPlayerTimer > TimeLimit)
        {            
            Vector2 playerPosVector = new Vector2(player.position.x, player.position.z);
            Vector2 searchVector;
            Vector3 newVector;
            float tempHeight;

            // randomoidaan lokaatioita xz-akseleilla ja haetaan niille oikeat korkeudet terrainista

            seed = Random.Range(8, 40);
            searchVector = playerPosVector + (Random.insideUnitCircle * seed);
            tempHeight = terrain.SampleHeight(searchVector);
            newVector = new Vector3(searchVector.x, tempHeight, searchVector.y);
            Monster.LastKnownPlayerPosition = newVector;
            Monster.OriginalPos = newVector;
            Monster.CurrentState = Monster.MonsterState.Investigate;
            lastSeenPlayerTimer = 0;
        }
	}
}
