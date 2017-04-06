using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour {

    public float interactDistance = 5f;
    public Inventory inventory;
    public UIManager uiManager;

    void Update ()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            string tag = hit.collider.tag;
            if (tag == "Item" || tag == "Interactable") {
                uiManager.ShowInteractBtn();
            }
            else {
                uiManager.HideInteractBtn();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (tag == "Item")
                {
                    inventory.AddItem(hit.collider.gameObject);
                }
                else if (tag == "Interactable")
                {         
                    string name = hit.collider.name;
                    switch (name)
                    {
                        // Interact with door
                        case "Door":
                            Door door = hit.collider.transform.parent.GetComponent<Door>();
                            // Check if player has the key
                            if (inventory.hasKey)
                                door.ChangeDoorState();
                            else
                                uiManager.ShowInfo("The door is locked. It looks old and could be broken with something. Or perhaps the key is still around.");
                            break;

                        // Interact with boat
                        case "Boat":
                            Boat boat = hit.collider.transform.GetComponent<Boat>();
                            if (!boat.hasMotor)
                            {
                                if (inventory.hasBoatMotor)
                                    uiManager.ShowInfo("Attach motor to the boat? <OK BUTTON HERE>");
                                else
                                    uiManager.ShowInfo("It's an old boat, but it still seems to be intact. The motor is missing however.");
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        }
        else
        {
            uiManager.HideInteractBtn();
            uiManager.HideInfo();
        }
    }

}
