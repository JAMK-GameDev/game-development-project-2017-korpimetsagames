using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject player;
    public Transform boat;
    bool isSailing;

    public Image fadeOutUIImage;
    public float fadeSpeed = -10f;
    public enum FadeDirection
    {
        In,
        Out
    }

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
        StartCoroutine(Fade(FadeDirection.In));
    }

    private IEnumerator Fade(FadeDirection fadeDirection)
    {
        float alpha = (fadeDirection == FadeDirection.Out) ? 1 : 0;
        float fadeEndValue = (fadeDirection == FadeDirection.Out) ? 0 : 1;
        if (fadeDirection == FadeDirection.Out)
        {
            while (alpha >= fadeEndValue)
            {
                SetColorImage(ref alpha, fadeDirection);
                yield return null;
            }
            fadeOutUIImage.enabled = false;
        }
        else
        {
            fadeOutUIImage.enabled = true;
            while (alpha <= fadeEndValue)
            {
                SetColorImage(ref alpha, fadeDirection);
                yield return null;
            }
        }
    }
    private void SetColorImage(ref float alpha, FadeDirection fadeDirection)
    {
        fadeOutUIImage.color = new Color(fadeOutUIImage.color.r, fadeOutUIImage.color.g, fadeOutUIImage.color.b, alpha);
        alpha += Time.deltaTime * (1.0f / fadeSpeed) * ((fadeDirection == FadeDirection.Out) ? -1 : 1);
    }
}
