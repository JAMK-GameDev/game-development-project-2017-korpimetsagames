using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class Underwater : MonoBehaviour {

    public bool isUnderwater;
	public float waterLevel = 0;

	//private Color uColor = new Color(0, 0, 0, 1);
	private float uDensity = .05f;
	
	//private Color aColor = new Color(0, 0, 0, 1);
	private float aDensity = .008f;
	
	public Renderer waterSurface;
	public Renderer underSurface;
	
	private Bloom bloomEffect;
	private Blur blurEffect;

    private AudioSource audioSrc;
    public AudioClip audioUnderwater;

	// Use this for initialization.
	void Start ()
	{
		bloomEffect = GetComponent<Bloom>();
		blurEffect = GetComponent<Blur>();
        audioSrc = GetComponent<AudioSource>();
    }

	// Update is called once per frame.
	void Update () {
		if (waterLevel < transform.position.y)
		{
            isUnderwater = false;
            audioSrc.Stop();
            RenderSettings.fogDensity = aDensity;
			//RenderSettings.fogColor = aColor;

			bloomEffect.enabled = true;
			blurEffect.enabled = false;
			
			waterSurface.enabled = true;
			underSurface.enabled = false;
		}
		else
		{
            isUnderwater = true;
            audioSrc.clip = audioUnderwater;
            if (!audioSrc.isPlaying) audioSrc.Play();
            RenderSettings.fogDensity = uDensity;
			//RenderSettings.fogColor = uColor;

			bloomEffect.enabled = false;
			blurEffect.enabled = true;
			
			waterSurface.enabled = false;
			underSurface.enabled = true;
		}
	}
}