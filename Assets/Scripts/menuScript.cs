using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class menuScript : MonoBehaviour {

    public Button playButton;
    public Button quitButton;
    public Image logo;
    public GameObject fearCanvas;
    public GameObject crosshair;
    FirstPersonController controller;

	void Start () {
        crosshair.SetActive(false);
        fearCanvas.SetActive(false);
        Time.timeScale = 0;
        controller = GameObject.Find("Player").GetComponent<FirstPersonController>();
        controller.DisableMouseLook(true);
        controller.enabled = false;
        playButton = playButton.GetComponent<Button>();
        quitButton = quitButton.GetComponent<Button>();
	}

    public void PlayPress()
    {
        GameObject.Find("MenuSounds").GetComponent<AudioSource>().Play();
        crosshair.SetActive(true);
        fearCanvas.SetActive(true);
        Time.timeScale = 1;
        gameObject.SetActive(false);
        controller.enabled = true;
        controller.DisableMouseLook(false);
        GameObject.FindObjectOfType<Inventory>().GiveFlashlight();
    }
	
    public void ExitPress()
    {
        playButton.enabled = true;
        quitButton.enabled = true;
        logo.enabled = true;
    }

    public void CreditsPress()
    {
        playButton.enabled = false;
        quitButton.enabled = false;
        logo.enabled = false;
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
