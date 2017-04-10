using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour {

    public GameObject equippedItem;
    public GameObject bulletPrefab;
    Vector3 bulletSpawnPos = new Vector3(0.08f, 10.7f, 36.8f);
    public Inventory inventory;
    public bool isAttacking { get; private set; }

	void Update () {   
        if (Input.GetMouseButtonDown(0) && equippedItem != null && equippedItem.GetComponent<ItemData>().canAttack && !inventory.inventoryIsOpen)
        {
            // Melee
            if (equippedItem.GetComponent<ItemData>().isMeleeWep)
            {
                equippedItem.GetComponent<AudioSource>().Play();
                Animation anim = equippedItem.GetComponent<Animation>();
                anim.Play("Attack");
                isAttacking = true;
                StartCoroutine(StopAttacking(anim.clip.length));
            }
            // Ranged
            else if (equippedItem.GetComponent<ItemData>().isRangedWep)
            {
                equippedItem.GetComponent<AudioSource>().Play();
                Fire();
            }
        } 
    }

    IEnumerator StopAttacking(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

    void Fire()
    {
        var bullet = (GameObject)Instantiate(bulletPrefab);
        bullet.transform.parent = equippedItem.transform;
        bullet.transform.localPosition = bulletSpawnPos;
        bullet.transform.localRotation = Quaternion.identity;
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 40;
        Destroy(bullet, 2.0f);
    }

}
