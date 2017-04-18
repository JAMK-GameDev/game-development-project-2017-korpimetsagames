using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFlash : MonoBehaviour {

    public float timeToDestroy = .5f;

    void Start()
    {
        Destroy();
    }

    void Destroy()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
