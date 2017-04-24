using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class menuScript : MonoBehaviour {

    public Canvas quitMenu;
    public Button playButton;
    public Button quitButton;
    public Button creditsButton;
    CursorLockMode wantedMode;
    public Texture backgroundTexture;
    public Canvas creditsWindow;
    public Image krampusLogo;
    public Button exitButton;

	// Use this for initialization
	void Start () {
        wantedMode = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = false;
        playButton = playButton.GetComponent<Button>();
        quitButton = quitButton.GetComponent<Button>();
        creditsButton = creditsButton.GetComponent<Button>();

        creditsWindow.enabled = false;
        exitButton.enabled = false;
	}

    void Update()
    {
    }

    public void PlayPress()
    {
        Time.timeScale = 1;
        Destroy(GameObject.Find("MainMenu"));
        wantedMode = CursorLockMode.Locked;
        Cursor.visible = false;
        GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = true;
    }
	
    public void ExitPress()
    {
        creditsWindow.enabled = false;
        playButton.enabled = true;
        quitButton.enabled = true;
        exitButton.enabled = false;
        krampusLogo.enabled = true;
    }

    public void CreditsPress()
    {
        creditsWindow.enabled = true;
        playButton.enabled = false;
        quitButton.enabled = false;
        krampusLogo.enabled = false;
        exitButton.enabled = true;
    }

    

    public void NoPress()
    {
        quitMenu.enabled = false;
        playButton.enabled = true;
        quitButton.enabled = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
