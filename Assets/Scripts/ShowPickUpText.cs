using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPickUpText : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject.Find("PickUpText").GetComponent<Text>().text = "E to pick up";
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject.Find("PickUpText").GetComponent<Text>().text = "";
        }
    }

    public void Disable()
    {
        GameObject.Find("PickUpText").GetComponent<Text>().text = "";
        gameObject.SetActive(false);
    }
}
