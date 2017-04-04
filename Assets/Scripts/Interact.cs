using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour {

    public float interactDistance = 5f;
    public Inventory inventory;
    public UIManager uiManager;

    void Update ()
    {
		if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                // Door interaction
                if (hit.collider.CompareTag("Door"))
                {
                    Door door = hit.collider.transform.parent.GetComponent<Door>();
                    if (door == null) return;

                    // Check if player has the key
                    if (inventory.hasKey)
                    {
                        door.ChangeDoorState();
                    }
                    else
                    {
                        uiManager.ShowInfo("Door is locked", 3);
                    }
                }
            }
        }
	}
}
