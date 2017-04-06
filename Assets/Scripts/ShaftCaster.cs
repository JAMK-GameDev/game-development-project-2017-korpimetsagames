using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaftCaster : MonoBehaviour {
    public Transform SunTransform;
    public Transform cameraTransform;
    public Transform objectTransform;
    public int casterDistance = 1024;
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        objectTransform.position = cameraTransform.position - SunTransform.forward * casterDistance;
    }
}