using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using System.Linq;

public class Inventory : MonoBehaviour {

    public GameObject inventoryUI = null;
    public GameObject inventoryUIButton = null;
    public Text inventoryUIMessage = null;
    public Transform inventoryUIContent = null;
    public Light flashlight = null;
    public FirstPersonController controller = null;
    List<GameObject> items = new List<GameObject>();
    Vector3 eqippedItemPos = new Vector3(1.27f, -0.65f, 1.38f);
    public GameObject equippedItem { get; private set; }
    public bool hasFlashlight { get; private set; }
    public bool hasKey { get; private set; }
    public bool hasBoatMotor { get; private set; }
    public bool inventoryIsOpen { get; private set; }
    public CombatSystem combatSystem;

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
            if (hasFlashlight && equippedItem.name != "Flashlight" )
            {
                GameObject flashlight = items.Where(obj => obj.name == "Flashlight").SingleOrDefault();
                EquipItem(flashlight); 
            }
        }
    }

    public void AddItem(GameObject item)
    {
        items.Add(item);
        AddinventoryUIButton(item);
        if (item.name == "Flashlight") { hasFlashlight = true; }
        if (item.name == "Key") { hasKey = true; }
        if (equippedItem == null && item.GetComponent<ItemData>().canEquip)
        {
            EquipItem(item);
        }
        else
        {
            item.SetActive(false);
            item.transform.parent = transform;
        }
    }

    public void EquipItem(GameObject item)
    {
        // Check if item can be equipped
        if (!item.GetComponent<ItemData>().canEquip)
        {
            StartCoroutine(ShowMessage("Cant equip that!", 2));
            return;
        }

        // Unequip previous item
        if (equippedItem != null)
        {
            UnequipItem(equippedItem);
        }

        // Equip the item
        item.SetActive(true);
        item.layer = LayerMask.NameToLayer("Equipped"); // Change layer to prevent clipping
        item.transform.parent = GameObject.FindGameObjectWithTag("MainCamera").transform;
        item.transform.localRotation = Quaternion.identity;
        item.transform.localPosition = eqippedItemPos;
        equippedItem = item;
        combatSystem.equippedItem = item;

        // Enable flashlight effect if flashlight item was equipped
        if (equippedItem.gameObject.name == "Flashlight" && flashlight != null)
        {
            flashlight.enabled = true;
        }
    }

    public void UnequipItem(GameObject item)
    {
        item.SetActive(false);
        item.transform.parent = transform;
        equippedItem = null;

        // Disable flashlight effect if flashlight item was unequipped
        if (item.gameObject.name == "Flashlight" && flashlight != null)
        {
            flashlight.enabled = false;
        }
    }

    void ToggleInventoryUI()
    {
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            if (inventoryUI.activeSelf)
            {
                inventoryIsOpen = true;
                if (controller != null)
                {
                    controller.DisableMouseLook(true);
                }
            }
            else
            {
                inventoryIsOpen = false;
                if (controller != null)
                {
                    controller.DisableMouseLook(false);
                }
            }
        }
    }

    public void AddinventoryUIButton(GameObject item)
    {
        GameObject go = Instantiate(inventoryUIButton) as GameObject;
        Sprite itemSprite = Resources.Load<Sprite>(item.name);
        if (itemSprite != null) {
            go.GetComponent<Image>().sprite = itemSprite;
        }
        else {
            go.GetComponentInChildren<Text>().text = item.name;
        }         
        go.GetComponent<Button>().onClick.AddListener(delegate{ EquipItem(item); });
        go.transform.SetParent(inventoryUIContent, false);
    }

    IEnumerator ShowMessage(string message, float delay)
    {
        inventoryUIMessage.text = message;
        yield return new WaitForSeconds(delay);
        inventoryUIMessage.text = "";
    }

}
