using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerBehavior : MonoBehaviour {

    private Transform player;
    UnityStandardAssets.Characters.FirstPerson.FirstPersonController controller;
    private float walkSpeed;
    private float runSpeed;
    public Transform monster;
    private RaycastHit hit;
    private Ray ray;
    public float fearMultiplier;
    private readonly int MAX_ANGLE = 50;
    Vector3 targetDir;

    // Use this for initialization
    void Start () {
        Player.FearLevel = 0;
        Player.CurrentState = Player.State.Carefree;
        controller = GameObject.FindObjectOfType<FirstPersonController>();
        player = transform;
        walkSpeed = controller.walkSpeed;
        runSpeed = controller.runSpeed;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //print(Player.FearLevel + ", state: " + Player.CurrentState + ", " + (int)Player.CurrentState);
        CheckForMonster();
        UpdatePlayerState();
    }

    private void CheckForMonster()
    {
        targetDir = monster.position - player.position;
        ray = new Ray(player.position, targetDir);

        // jos säde osuu hirviöön && hirviö on pelaajan edessä edessä
        if (Vector3.Angle(player.forward, targetDir) <= MAX_ANGLE &&
            Physics.Raycast(ray, out hit) &&
            hit.collider.tag.Equals("Enemy"))
        {
            Player.FearLevel += fearMultiplier * Time.deltaTime;
        }
        // jos pelaaja ei näe hirviötä
        else
        {
            Player.FearLevel -= fearMultiplier / 2 * Time.deltaTime;
        }
    }

    private void UpdatePlayerState()
    {
        // jos täytyy siirtyä pelokkaampaan tasoon
        if (Player.FearLevel > 100 && Player.CurrentState != Player.State.Screaming)
        {
            Player.WorsenState();
            Player.FearLevel = Player.FearLevel % 100;
        }
        // jos täytyy siirtyä huolettomampaan tasoon
        else if (Player.FearLevel < 0 && Player.CurrentState != Player.State.Carefree)
        {
            Player.ImproveState();
            Player.FearLevel = 99;
        }
        // jos maksimi peloissaan, ei nosteta enempää pelkoa
        else if (Player.FearLevel > 100 && Player.CurrentState == Player.State.Screaming)
        {
            Player.FearLevel = 99;
        }
        // jos pelaaja on huoleton, ei lasketa enempää pelkoa
        else if (Player.FearLevel < 0 && Player.CurrentState == Player.State.Carefree)
        {
            Player.FearLevel = 1;
        }

        if((int)Player.CurrentState >= (int)Player.State.Paralyzed)
        {
            controller.walkSpeed = walkSpeed / 4;
            controller.runSpeed = walkSpeed / 4;
        }
        else
        {
            controller.walkSpeed = walkSpeed;
            controller.runSpeed = runSpeed;
        }
    }
}
