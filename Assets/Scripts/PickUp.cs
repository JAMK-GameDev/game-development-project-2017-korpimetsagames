using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour
{
    float distanceToItem = 2f;
    Vector3 eqippedItemPos = new Vector3(1.27f, -0.65f, 1.38f);

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
                    transform.parent = GameObject.FindGameObjectWithTag("MainCamera").transform;
                    transform.localRotation = Quaternion.identity;
                    transform.localPosition = eqippedItemPos;
                    transform.FindChild("PickUpZone").GetComponent<ShowPickUpText>().Disable();
                }
            }
        }
    }

}