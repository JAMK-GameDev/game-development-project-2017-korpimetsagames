using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.ImageEffects;

public class GameManager : MonoBehaviour {

    public GameObject player;
    public Transform boat;
    public GameObject victoryScreen;
    public GameObject defeatScreen;
    public Text fearDescription;
    public Slider fearLevel;
    public VignetteAndChromaticAberration deathEffect;
    public GameObject cheatConsole;
    public Inventory inventory;

    bool isSailing;
    bool isDead;
    public bool gameOver { get; private set; }

    float lerpTime = 5f;
    float currentLerpTime;

    public bool cheatConsoleIsOpen;
    public bool godmode;

    void Update ()
    {
        // Cheats
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCheatConsole();
        }
        if (godmode)
        {
            if (player.GetComponent<FirstPersonController>().walkSpeed < 60)
            {
                player.GetComponent<FirstPersonController>().walkSpeed = 60;
                player.GetComponent<FirstPersonController>().runSpeed = 60;
            }
        }

        // Move boat
        if (isSailing)
        {
            boat.transform.position = Vector3.MoveTowards(boat.transform.position, new Vector3(boat.position.x, boat.position.y, boat.position.z + 100), 2f * Time.deltaTime);
        }

        // Show death effect
        if (isDead)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }
            float perc = currentLerpTime / lerpTime;
            deathEffect.intensity = Mathf.Lerp(0, 1, perc);
            deathEffect.blur = Mathf.Lerp(0, 1, perc * 2);
        }
        switch (Player.Psyche)
        {
            case Player.PsycheState.Carefree: fearDescription.text = "Carefree"; break;
            case Player.PsycheState.Stressed: fearDescription.text = "Stressed"; break;
            case Player.PsycheState.Panic: fearDescription.text = "Panic"; break;
            case Player.PsycheState.Paralyzed: fearDescription.text = "Paralyzed"; break;
            case Player.PsycheState.Berserk: fearDescription.text = "Berserk"; break;
        }
        fearLevel.value = Player.FearLevel;
    }

    public void EndingKillMonster()
    {
        StartCoroutine(EndGame(true, 4f));
    }

    public void EndingBoat()
    {
        player.transform.parent = boat.transform;
        player.transform.localPosition = new Vector3(0, 1.5f, 0.8f);
        player.GetComponent<CharacterController>().enabled = false;
        isSailing = true;
        StartCoroutine(EndGame(true, 15f));
    }

    public void EndingDie()
    {
        GetComponent<AudioSource>().Play();
        player.GetComponent<CharacterController>().enabled = false;
        isDead = true;
        StartCoroutine(EndGame(false, 1f));
    }

    IEnumerator EndGame(bool won, float delay)
    {
        yield return new WaitForSeconds(delay);
        isSailing = false;
        player.GetComponent<FirstPersonController>().DisableMouseLook(true);
        gameOver = true;
        if (won)
        {
            victoryScreen.SetActive(true);
        }
        else
        {
            defeatScreen.SetActive(true);
        }
        StartCoroutine(EndHeartbeat());   
    }

    IEnumerator EndHeartbeat()
    {
        yield return new WaitForSeconds(6f);
        GetComponent<AudioSource>().Stop();
    }

    public void Restart()
    {
        GetComponent<AudioSource>().Stop();
        SceneManager.LoadScene("Island");
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void ToggleCheatConsole()
    {
        cheatConsole.SetActive(!cheatConsole.activeSelf);
        cheatConsoleIsOpen = cheatConsole.activeSelf;
        player.GetComponent<FirstPersonController>().DisableMouseLook(cheatConsole.activeSelf);
    }

    #region CHEATS
    public void Godmode(bool b)
    {
        godmode = b;
    }

    public void AddCrowbar()
    {
        GameObject crowbar = GameObject.Find("crowbar");
        inventory.AddItem(crowbar);
    }

    public void AddShotgun()
    {
        GameObject shotty = GameObject.Find("shotty");
        inventory.AddItem(shotty);
    }

    public void AddDoorKey()
    {
        GameObject key = GameObject.Find("Key");
        inventory.AddItem(key);
    }
    #endregion
}
