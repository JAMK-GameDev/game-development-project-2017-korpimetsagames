using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public bool isOpen = false;
    public Quaternion originalRotation;
    public float speed = 2f;
    public AudioClip openSound;

	void Start ()
    {
        originalRotation = gameObject.transform.rotation;
	}
	
	void Update ()
    {
		if (isOpen)
        {
            Quaternion targetRotation = Quaternion.Euler(originalRotation.x, originalRotation.y - 90, originalRotation.z);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, speed * Time.deltaTime);
        }
        else
        {
            Quaternion targetRotation2 = Quaternion.Euler(originalRotation.x, originalRotation.y, originalRotation.z);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation2, speed * Time.deltaTime);
        }
	}

    public void ChangeDoorState()
    {
        isOpen = !isOpen;
        GetComponent<AudioSource>().clip = openSound;
        GetComponent<AudioSource>().Play();
    }
}
