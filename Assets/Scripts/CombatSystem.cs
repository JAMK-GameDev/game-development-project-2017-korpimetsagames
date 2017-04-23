using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour {

    public GameObject equippedItem;
    public Inventory inventory;
    public GameManager gameManager;

    public AudioClip shoot;
    public AudioClip reload;
    public GameObject muzzleFlash;

    public bool isAttacking { get; private set; }
    public float fireRate = 0.25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Transform gunEnd;
    public Camera fpsCam;
    private WaitForSeconds shotDuration = new WaitForSeconds(.07f);
    private float nextFire;
    public int ammoCount = 2;
    bool magazineEmpty;
    bool reloading;

    void Update () {   
        if (Input.GetMouseButtonDown(0) && equippedItem != null && !inventory.inventoryIsOpen && !gameManager.gameOver && !gameManager.cheatConsoleIsOpen)
        {
            // Melee
            if (equippedItem.GetComponent<ItemData>().isMeleeWep)
            {
                if (!isAttacking)
                    Attack();
            }
            // Ranged
            else if (equippedItem.GetComponent<ItemData>().isRangedWep)
            {
                if (Time.time > nextFire && !magazineEmpty) // check if enough time has passed since player last fired
                {
                    nextFire = Time.time + fireRate;
                    Shoot();
                }
                else if (magazineEmpty && !reloading)
                {
                    Reload();
                }
            }
        } 

        if (Input.GetKeyDown(KeyCode.R) && !reloading && magazineEmpty && equippedItem != null && equippedItem.GetComponent<ItemData>().isRangedWep)
        {
            Reload();
        }
    }

    void Attack()
    {
        equippedItem.GetComponent<AudioSource>().Play();
        Animation anim = equippedItem.GetComponent<Animation>();
        anim.Play("Attack");
        isAttacking = true;
        StartCoroutine(StopAttacking(anim.clip.length));
    }

    void Shoot()
    {
        if(Monster.CurrentState != Monster.MonsterState.Dead)
        {
            Monster.Mood = Monster.Mindset.Excited;
            Monster.LearnPlayerPosition(GameObject.FindGameObjectWithTag("Player").transform.position);
        }        
        equippedItem.GetComponent<AudioSource>().clip = shoot;
        equippedItem.GetComponent<AudioSource>().Play();
        Animation anim = equippedItem.GetComponent<Animation>();
        anim.Play("Shoot");
        
        Instantiate(muzzleFlash, gunEnd.transform.position, Quaternion.identity);
        ammoCount--;
        if (ammoCount == 0) { magazineEmpty = true; }
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
        {
            //Debug.Log(hit.collider.name);
            if(hit.collider.tag == "Enemy")
            {
                hit.transform.GetComponent<MonsterBehavior>().GetHit();
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * hitForce);
            }
        }
    }

    void Reload()
    {
        reloading = true;
        equippedItem.GetComponent<AudioSource>().clip = reload;
        equippedItem.GetComponent<AudioSource>().Play();
        Animation anim = equippedItem.GetComponent<Animation>();
        anim.Play("Reload");
        StartCoroutine(FinishReloading(2.4f));
    }

    IEnumerator FinishReloading(float delay)
    {
        yield return new WaitForSeconds(delay);
        reloading = false;
        ammoCount = 2;
        magazineEmpty = false;
    }

    IEnumerator StopAttacking(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

}
