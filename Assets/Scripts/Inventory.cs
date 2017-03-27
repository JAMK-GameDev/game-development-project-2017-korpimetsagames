using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using System.Linq;

public class Inventory : MonoBehaviour {

    public GameObject inventoryUI = null;
    public Transform inventoryUIContent = null;
    public GameObject flashlight = null;
    public FirstPersonController controller = null;
    public GameObject uiButton = null;
    List<GameObject> items = new List<GameObject>();
    GameObject equippedItem = null;
    Vector3 eqippedItemPos = new Vector3(1.27f, -0.65f, 1.38f);
    bool hasFlight = false;

    void Start()
    {
        ToggleInventoryUI(); // Disable inventory UI at start
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventoryUI();
        }

        // Hotkey to equip flashlight
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (hasFlight && equippedItem.name != "Flashlight" )
            {
                GameObject flashlight = items.Where(obj => obj.name == "Flashlight").SingleOrDefault();
                EquipItem(flashlight);
            }
        }
    }

    public void AddItem(GameObject item)
    {
        items.Add(item);
        AddUIButton(item);
        if (item.name == "Flashlight") { hasFlight = true; }
        if (equippedItem == null)
        {
            EquipItem(item);
        }
        else
        {
            item.SetActive(false);
            item.transform.parent = GameObject.FindGameObjectWithTag("Inventory").transform;
        }
    }

    public void EquipItem(GameObject item)
    {
        if (equippedItem != null)
        {
            UnequipItem(equippedItem);
        }

        item.SetActive(true);
        item.transform.parent = GameObject.FindGameObjectWithTag("MainCamera").transform;
        item.transform.localRotation = Quaternion.identity;
        item.transform.localPosition = eqippedItemPos;
        equippedItem = item;

        // Enable flashlight effect
        if (equippedItem.gameObject.name == "Flashlight" && flashlight != null)
        {
            flashlight.SetActive(true);
        }
    }

    public void UnequipItem(GameObject item)
    {
        item.SetActive(false);
        item.transform.parent = GameObject.FindGameObjectWithTag("Inventory").transform;
        equippedItem = null;

        // Disable flashlight effect
        if (item.gameObject.name == "Flashlight" && flashlight != null)
        {
            flashlight.SetActive(false);
        }
    }

    void ToggleInventoryUI()
    {
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            if (inventoryUI.activeSelf)
            {
                if (controller != null)
                {
                    controller.DisableMouseLook(true);
                }
            }
            else
            {
                if (controller != null)
                {
                    controller.DisableMouseLook(false);
                }
            }
        }
    }

    public void AddUIButton(GameObject item)
    {
        GameObject go = Instantiate(uiButton) as GameObject;
        go.GetComponentInChildren<Text>().text = item.name;
        go.GetComponent<Button>().onClick.AddListener(delegate{ EquipItem(item); });
        go.transform.SetParent(inventoryUIContent, false);
    }

}
