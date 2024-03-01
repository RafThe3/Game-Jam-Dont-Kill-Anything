using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private GameObject hand;
    [Min(0), SerializeField] private float pickupDistance = 1;
    [SerializeField] private TMPro.TextMeshProUGUI interactText;

    private void Update()
    {
        Ray pickupDirection = new(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(pickupDirection, out RaycastHit hit, pickupDistance))
        {
            Debug.Log("Can pickup object");

            if (Input.GetKeyDown(KeyCode.E) && hit.collider.CompareTag("Item"))
            {
                PickupObject(hit);
            }
        }
    }

    private void PickupObject(RaycastHit hit)
    {
        Quaternion resetRotation = new(0, 0, 0, 0);
        GameObject clone = Instantiate(hit.collider.gameObject, hand.transform.position, resetRotation, hand.transform);
        clone.GetComponent<Rigidbody>().isKinematic = true;
        Destroy(hit.collider.gameObject);
    }
}
