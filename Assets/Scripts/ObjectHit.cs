using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHit : MonoBehaviour {

    public int hitPoints = 4;
    public CombatSystem combatSystem;
    public AudioSource hitSound;
    public AudioSource breakSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            if (combatSystem.isAttacking)
            {
                if (hitPoints <= 0)
                {
                    Debug.Log("OVI PASKAKS!!!");
                    if (!breakSound.isPlaying) breakSound.Play();
                    StartCoroutine(Destroy(transform.parent.gameObject, breakSound.clip.length));
                }
                else
                {
                    hitPoints--;
                    if (!hitSound.isPlaying) hitSound.Play();
                }
            }
        }
    }

    IEnumerator Destroy(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(go);
    }
}
