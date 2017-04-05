using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour {

    public GameObject equippedItem;
	
	void Update () {

        if (Input.GetMouseButtonDown(0) && equippedItem != null && equippedItem.GetComponent<ItemData>().canAttack)
        {
            Debug.Log("ATTACCCKK!!!");
            equippedItem.GetComponent<Animation>().Play("Attack");
        }
       
    }
}
