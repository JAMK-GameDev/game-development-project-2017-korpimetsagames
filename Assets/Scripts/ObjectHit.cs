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
                    if (!breakSound.isPlaying) breakSound.Play();
                    StartCoroutine(Destroy(transform.gameObject, breakSound.clip.length));
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
        // Play ebin destruction anim here
        Destroy(go);
    }
}
