using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    List<GameObject> items = new List<GameObject>();
    bool itemEquipped = false;
    Vector3 eqippedItemPos = new Vector3(1.27f, -0.65f, 1.38f);

    public void addItem(GameObject item)
    {
        items.Add(item);
        item.transform.parent = gameObject.transform;
        if (!itemEquipped)
        {
            equipItem(item);
        }   
    }

    void equipItem(GameObject item)
    {
        item.transform.parent = GameObject.FindGameObjectWithTag("MainCamera").transform;
        item.transform.localRotation = Quaternion.identity;
        item.transform.localPosition = eqippedItemPos;
        itemEquipped = true;
    }
}
