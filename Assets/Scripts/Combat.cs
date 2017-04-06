using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour {

    public GameObject equippedItem;
    Inventory inventory;

    void Start() {
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
    }
    
	void Update () {   
        if (Input.GetMouseButtonDown(0) && equippedItem != null && equippedItem.GetComponent<ItemData>().canAttack)
        {
            if (!inventory.inventoryIsOpen)
                equippedItem.GetComponent<Animation>().Play("Attack");
        } 
    }
}
