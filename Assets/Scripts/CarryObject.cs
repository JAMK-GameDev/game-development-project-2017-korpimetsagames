using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryObject : MonoBehaviour
{
    bool isCarrying;
    public bool isStationary;
    public PlayerBehavior player;
    public Inventory inventory;

    void Update()
    {
        if (isCarrying)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StopCarry();
            }
        }
    }

    public void Carry()
    {
        if (!isStationary)
        { 
            isCarrying = true;
            gameObject.tag = "Untagged";
            if (gameObject.name == "Motor") { inventory.hasBoatMotor = true; }
            player.CarryObject(); // Slow down player
            inventory.CarryObject(gameObject); // Move the object to camera
        }
    }

    public void StopCarry()
    {
        if (!isStationary)
        {
            isCarrying = false;
            gameObject.tag = "Interactable";
            if (gameObject.name == "Motor") { StartCoroutine(DeleteMotorFromInventory()); }
            player.StopCarryObject();
            inventory.DropObject(gameObject);
        }
    }

    // Wait a bit before deleting so boat has time to register motor attaching
    IEnumerator DeleteMotorFromInventory()
    {
        yield return new WaitForSeconds(2);
        inventory.hasBoatMotor = false;
    }
}