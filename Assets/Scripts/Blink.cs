using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Blink : MonoBehaviour
{
    public Image img;
    bool fadeState = false;

    void Update()
    {
        if (fadeState) FadeIn();
        else FadeOut();
    } 

    void FadeIn()
    {
        img.CrossFadeAlpha(1.0f, 0.8f, true);
        if (img.canvasRenderer.GetAlpha() > 0.9f)
        {
            fadeState = false;
        }
    }

    void FadeOut()
    {
        img.CrossFadeAlpha(0.0f, 0.8f, true);
        if (img.canvasRenderer.GetAlpha() < 0.1f)
        {
            fadeState = true;
        }
    }
}