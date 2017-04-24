using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour {

    public Light flashlight;
    public AudioClip soundOn;
    public AudioClip soundOff;
    public Inventory inventory;

    void Start ()
    {
        flashlight = GetComponent<Light>();
        flashlight.enabled = false;
    }

	void Update ()
    {
		if (inventory.hasFlashlight && Input.GetKeyDown(KeyCode.F))
        {
            if (!flashlight.enabled)
            {
                flashlight.enabled = true;
                // TODO: Play sound
            }
            else
            {
                flashlight.enabled = false;
                // TODO: Play sound
            }
        }
	}
}
