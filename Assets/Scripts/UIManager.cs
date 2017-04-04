using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject info;
    public Text infoText;

    public void ShowInfo(string msg)
    {
        info.SetActive(true);
        infoText.text = msg;
    }

    public void HideInfo()
    {
        infoText.text = "";
        info.SetActive(false);
    }

    public void ShowInfo(string msg, float duration)
    {
        StartCoroutine(ShowInfoForDuration(msg, duration));
    }

    IEnumerator ShowInfoForDuration(string msg, float duration)
    {
        info.SetActive(true);
        infoText.text = msg;
        yield return new WaitForSeconds(duration);
        HideInfo();
    }

}
