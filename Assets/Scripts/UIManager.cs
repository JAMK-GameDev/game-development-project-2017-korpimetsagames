using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject info;
    public Text infoText;
    public GameObject interactBtn;
    bool showInfo;

    public void ShowInfo(string msg)
    {
        HideInteractBtn();
        showInfo = true;
        info.SetActive(true);
        infoText.text = msg;
    }

    public void ShowInfo(string msg, float duration)
    {
        HideInteractBtn();
        showInfo = true;
        StartCoroutine(ShowInfoForDuration(msg, duration));
    }

    public void HideInfo()
    {
        infoText.text = "";
        showInfo = false;
        info.SetActive(false);
    }

    public void ShowInteractBtn()
    {
        if (!showInfo)
            interactBtn.SetActive(true);
    }

    public void ShowInteractBtn(float duration)
    {
        if (!showInfo)
        {
            interactBtn.SetActive(true);
            StartCoroutine(HideInteractBtn(duration));
        }
    }

    public void HideInteractBtn()
    {
        interactBtn.SetActive(false);
    }

    IEnumerator ShowInfoForDuration(string msg, float duration)
    {
        info.SetActive(true);
        infoText.text = msg;
        yield return new WaitForSeconds(duration);
        HideInfo();
    }

    IEnumerator HideInteractBtn(float duration)
    {
        yield return new WaitForSeconds(duration);
        interactBtn.SetActive(false);
    }

}
