using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour {

    Light flight;
    public AudioClip soundOn;
    public AudioClip soundOff;

    void Start ()
    {
        flight = GetComponent<Light>();
        flight.enabled = false;
    }

	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.F))
        {
            if (!flight.enabled)
            {
                flight.enabled = true;
                // TODO: Play sound
            }
            else
            {
                flight.enabled = false;
                // TODO: Play sound
            }
        }
	}
}
