using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    bool inZone = false;

    void Update()
    {
        Collect();
    }

    void Collect()
    {
        if (Input.GetKeyDown(KeyCode.E) && inZone)
        {
            GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>().AddItem(gameObject);
            DisableText();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject.Find("PickUpText").GetComponent<Text>().text = "E to pick up";
            inZone = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject.Find("PickUpText").GetComponent<Text>().text = "";
            inZone = false;
        }
    }

    void DisableText()
    {
        GetComponent<Collider>().enabled = false;
        GameObject.Find("PickUpText").GetComponent<Text>().text = "";
        inZone = false;
    }

}