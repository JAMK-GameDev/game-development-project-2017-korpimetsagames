using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour {

    public GameObject equippedItem;
    Inventory inventory;
    public bool isAttacking { get; private set; }

    void Start() {
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
    }
    
	void Update () {   
        if (Input.GetMouseButtonDown(0) && equippedItem != null && equippedItem.GetComponent<ItemData>().canAttack)
        {
            if (!inventory.inventoryIsOpen)
            {
                equippedItem.GetComponent<AudioSource>().Play();
                Animation anim = equippedItem.GetComponent<Animation>();
                anim.Play("Attack");
                isAttacking = true;
                StartCoroutine(StopAttacking(anim.clip.length));
            }
        } 
    }

    IEnumerator StopAttacking(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }
}
