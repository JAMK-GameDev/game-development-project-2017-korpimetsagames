using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Underwater : MonoBehaviour {

    public int waterLevel = 930;
	
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y < waterLevel - 1)
        {
            RenderSettings.fog = true;
            RenderSettings.fogDensity = 0.2f;
        }
        else
        {
            RenderSettings.fog = false;
            RenderSettings.fogDensity = 0.0f;
        }
    }
}