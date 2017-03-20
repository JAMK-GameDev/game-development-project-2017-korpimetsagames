using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public GameObject inventoryUI = null;
    List<GameObject> items = new List<GameObject>();
    GameObject equippedItem = null;
    Vector3 eqippedItemPos = new Vector3(1.27f, -0.65f, 1.38f);

    void Start()
    {
        ToggleInventoryUI(); // Disable inventory UI
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventoryUI();
        }
    }

    public void AddItem(GameObject item)
    {
        items.Add(item);
        item.transform.parent = gameObject.transform;
        if (equippedItem == null)
        {
            EquipItem(item);
        }   
    }

    void EquipItem(GameObject item)
    {
        item.SetActive(true);
        item.transform.parent = GameObject.FindGameObjectWithTag("MainCamera").transform;
        item.transform.localRotation = Quaternion.identity;
        item.transform.localPosition = eqippedItemPos;
        equippedItem = item;
    }

    void UnequipItem(GameObject item)
    {
        item.SetActive(false);
        item.transform.parent = GameObject.FindGameObjectWithTag("Inventory").transform;
        equippedItem = null;
    }

    void ToggleInventoryUI()
    {
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }
}
