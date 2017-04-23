using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHit : MonoBehaviour {

    public int hitPoints = 4;
    public CombatSystem combatSystem;
    AudioSource src;
    public AudioClip hitSound;
    public AudioClip breakSound;

    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            if (combatSystem.isAttacking)
            {
                if (hitPoints <= 0)
                {
                    if (!src.isPlaying)
                    {
                        src.clip = breakSound;
                        src.Play();
                    }
                    StartCoroutine(Destroy(transform.gameObject, breakSound.length));
                }
                else
                {
                    hitPoints--;
                    if (!src.isPlaying)
                    {
                        src.clip = hitSound;
                        src.Play();
                    }
                }
            }
        }
    }

    IEnumerator Destroy(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        // Play destruction anim here
        Destroy(go);
    }
}
