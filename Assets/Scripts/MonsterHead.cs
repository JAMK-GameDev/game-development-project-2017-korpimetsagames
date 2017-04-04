using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHead : MonoBehaviour {

    private Transform head;
    public Transform body;
    public Transform player;
    private Vector3 targetPosition;
    private Vector3 targetDir;
    private int speed;
	// Use this for initialization
	void Start () {
        speed = 5;
        head = transform;
    }
	
	// Update is called once per frame
	void Update ()
    {
        float step = speed * Time.deltaTime;

        switch (Monster.CurrentState)
        {
            case Monster.MonsterState.Chase:
                targetPosition = player.position;
                targetPosition.y = targetPosition.y + 1;
                targetDir = targetPosition - head.position;
                break;
            case Monster.MonsterState.Investigate:
                targetDir = body.forward;
                break;
            case Monster.MonsterState.Idle:
                targetDir = body.forward;
                break;
            case Monster.MonsterState.Search:
                targetDir = body.forward;
                break;
            case Monster.MonsterState.Survey:
                targetDir = body.forward;
                break;
        }        
        
        Vector3 newDir = Vector3.RotateTowards(head.forward, targetDir, step, 0.0F);        
        head.rotation = Quaternion.LookRotation(newDir);
    }
}
