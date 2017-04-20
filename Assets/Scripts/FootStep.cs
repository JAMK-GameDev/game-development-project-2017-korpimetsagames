using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep : MonoBehaviour {

    public float stepRate = 1f;
    public float stepCoolDown;
    public AudioClip footStep;

    // Update is called once per frame
    void Update()
    {
        if (Player.MoveMode == Player.MoveState.Walk)
        {
            stepRate = 0.9f;
        }
        else if (Player.MoveMode == Player.MoveState.Run)
        {
            stepRate = 0.5f;
        }

        stepCoolDown -= Time.deltaTime;
        if ((Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) && stepCoolDown < 0f && Player.MoveMode != Player.MoveState.Sneak)
        {
            GetComponent<AudioSource>().pitch = 1f + Random.Range(-0.2f, 0.2f);
            GetComponent<AudioSource>().PlayOneShot(footStep, 0.9f);
            stepCoolDown = stepRate;
        }
    }
}
