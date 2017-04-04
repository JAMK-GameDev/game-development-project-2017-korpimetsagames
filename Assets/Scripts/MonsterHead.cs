using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHead : MonoBehaviour {

    private Transform head;
    public Transform body;
    public Transform player;
    private Vector3 tempPosition;
    //private Vector3 tempDir;
    private Vector3 targetDir;
    private Vector3 newDir;
    private int speed;
    //private float maxTurnX, maxTurnY, maxTurnZ, minTurnX, minTurnY, minTurnZ;
	void Start () {
        speed = 5;
        head = transform;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // TODO: käännetään päätä random suuntaan. Jos 80 astetta ylittyy, käännetään kroppaa eikä päätä.
        float step = speed * Time.deltaTime;
        switch (Monster.CurrentState)
        {
            case Monster.MonsterState.Chase:
                tempPosition = player.position;
                tempPosition.y = tempPosition.y + (float)0.75;
                targetDir = tempPosition - head.position; // vektori johon pään pitää katsoa jotta se osoittaa pelaajaa. v2 - v1 = v3
                
             /*   targetDir = new Vector3(
                    Mathf.Clamp(tempDir.x, body.forward.x, body.forward.x),
                    Mathf.Clamp(tempDir.y, body.up.y, body.up.y),
                    Mathf.Clamp(tempDir.z, body.right.z, body.right.z));*/
                break;
            case Monster.MonsterState.Investigate:
                tempPosition = Monster.LastKnownPlayerPosition;
                tempPosition.y = tempPosition.y + (float)0.75;
                targetDir = tempPosition - head.position;
                break;                     
            case Monster.MonsterState.Survey:                
                targetDir = body.forward;
                break;
            case Monster.MonsterState.Search:
                targetDir = body.forward;
                break;
            case Monster.MonsterState.Idle:
                targetDir = body.forward;
                break;
        }   

        newDir = Vector3.RotateTowards(head.forward, targetDir, step, 0.0f);   
        head.rotation = Quaternion.LookRotation(newDir);
    }
}
