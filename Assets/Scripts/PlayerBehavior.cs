using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerBehavior : MonoBehaviour {

    private Transform player;
    FirstPersonController controller;
    Rigidbody rigidBody;
    private float walkSpeed;
    private float runSpeed;
    public Transform monster;
    private RaycastHit hit;
    private Ray ray;
    public float fearMultiplier;
    private readonly int MAX_ANGLE = 50;
    Vector3 targetDir;
    bool isCarryingObject;

    // Use this for initialization
    void Start () {
        Player.FearLevel = 0;
        Player.Psyche = Player.PsycheState.Carefree;
        Player.MoveMode = Player.MoveState.Walk;
        controller = GameObject.FindObjectOfType<FirstPersonController>();
        player = transform;
        walkSpeed = controller.walkSpeed;
        runSpeed = controller.runSpeed;
        player.hasChanged = false;
        Player.IsStationary = true;
        rigidBody = player.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKey(KeyCode.L))
        {
            GameObject.FindObjectOfType<FirstPersonController>().Die();
        }
        IsPlayerStationaryUpdate();
        CheckForMonster();
        UpdatePlayerPsyche();
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

    private void UpdatePlayerPsyche()
    {
        // jos täytyy siirtyä pelokkaampaan tasoon
        if (Player.FearLevel > 100 && Player.Psyche != Player.PsycheState.Berserk)
        {
            Player.WorsenState();
            Player.FearLevel = Player.FearLevel % 100;
        }
        // jos täytyy siirtyä huolettomampaan tasoon
        else if (Player.FearLevel < 0 && Player.Psyche != Player.PsycheState.Carefree)
        {
            Player.ImproveState();
            Player.FearLevel = 99;
        }
        // jos maksimi peloissaan, ei nosteta enempää pelkoa
        else if (Player.FearLevel > 100 && Player.Psyche == Player.PsycheState.Berserk)
        {
            Player.FearLevel = 99;
        }
        // jos pelaaja on huoleton, ei lasketa enempää pelkoa
        else if (Player.FearLevel < 0 && Player.Psyche == Player.PsycheState.Carefree)
        {
            Player.FearLevel = 1;
        }

        if((int)Player.Psyche >= (int)Player.PsycheState.Paralyzed && !isCarryingObject)
        {
            controller.walkSpeed = walkSpeed / 4;
            controller.runSpeed = walkSpeed / 4;
        }
        else if (!isCarryingObject)
        {
            controller.walkSpeed = walkSpeed;
            controller.runSpeed = runSpeed;
        }
    }

    private void IsPlayerStationaryUpdate()
    {
        if(rigidBody.IsSleeping())
        {
            Player.IsStationary = true;
        }
        else
        {
            Player.IsStationary = false;
        }
    }

    public void CarryObject()
    {
        isCarryingObject = true;
        controller.walkSpeed = 2;
        controller.runSpeed = 4;
    }

    public void StopCarryObject()
    {
        isCarryingObject = false;
        controller.walkSpeed = walkSpeed;
        controller.runSpeed = runSpeed;
    }
}
