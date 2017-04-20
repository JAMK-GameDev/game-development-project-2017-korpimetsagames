using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour {

    public GameObject player;
    public Transform boat;
    public GameObject victoryScreen;
    public GameObject defeatScreen;
    bool isSailing;

    void Update ()
    {
        if (isSailing)
        {
            boat.transform.position = Vector3.MoveTowards(boat.transform.position, new Vector3(boat.position.x, boat.position.y, boat.position.z + 100), 2f * Time.deltaTime);
        }
    }

    public void EndingBoat()
    {
        player.transform.parent = boat.transform;
        player.transform.localPosition = new Vector3(0, 1.5f, 0.8f);
        player.GetComponent<CharacterController>().enabled = false;
        isSailing = true;
        StartCoroutine(EndGame(true, 10f));
    }

    public void EndingDie()
    {
        StartCoroutine(EndGame(false, 1f));
    }

    IEnumerator EndGame(bool won, float delay)
    {
        yield return new WaitForSeconds(delay);
        isSailing = false;
        player.GetComponent<FirstPersonController>().DisableMouseLook(true);
        if (won)
        {
            victoryScreen.SetActive(true);
        }
        else
        {
            defeatScreen.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Island");
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

}
