using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour
{
    float distanceToItem = 2f;

    void Update()
    {
        Collect();
    }

    void Collect()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, distanceToItem))
            {
                if (hit.collider.gameObject.tag == "Item")
                {
                    GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>().AddItem(gameObject);
                    transform.FindChild("PickUpZone").GetComponent<ShowPickUpText>().Disable();
                }
            }
        }
    }

}