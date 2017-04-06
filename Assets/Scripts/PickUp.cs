using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour
{
    public UIManager uiManager;
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
            uiManager.ShowInfo("E to pick up");
            inZone = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            uiManager.HideInfo();
            inZone = false;
        }
    }

    void DisableText()
    {
        GetComponent<BoxCollider>().enabled = false; // Disable pick up zone
        uiManager.HideInfo();
        inZone = false;
    }

}