using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep : MonoBehaviour {

    public float stepRate = 1f;
    public float stepCoolDown;
    public AudioClip footStepTerrain;
    public AudioClip footStepFloor;
    AudioClip footStep;
    public Underwater uw;
    GameObject player;
    float distance = 100f;

    void Start()
    {
        player = transform.parent.gameObject;
    }

    void Update()
    {
        // Check surface material and play sound accordingly
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distance))
        {
            if (hit.collider.name == "Terrain")
            {
                footStep = footStepTerrain;
            }
            else
            {
                footStep = footStepFloor;
            }
        }

        if (Player.MoveMode == Player.MoveState.Walk)
        {
            stepRate = 0.9f;
        }
        else if (Player.MoveMode == Player.MoveState.Run)
        {
            stepRate = 0.5f;
        }

        stepCoolDown -= Time.deltaTime;
        if ((Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) && stepCoolDown < 0f && Player.MoveMode != Player.MoveState.Sneak && !uw.isUnderwater)
        {
            GetComponent<AudioSource>().pitch = 1f + Random.Range(-0.2f, 0.2f);
            GetComponent<AudioSource>().PlayOneShot(footStep, 0.9f);
            stepCoolDown = stepRate;
        }
    }
}
