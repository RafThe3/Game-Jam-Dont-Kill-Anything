using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private bool active = false;
    [SerializeField] private GameObject selection;

    void Start()
    {
        
    }
    void Update()
    {
        PlaceObjectHologram();
        if ((Input.GetKeyDown(KeyCode.E)))
        {
            PlaceObject();
        }
    }

    private void PlaceObjectHologram()
    {
        // test code, just to make sure this works lol
        if (!active)
        {
            return;
        }

        Ray shootDirection = new(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(shootDirection, out RaycastHit hit))
        {
            if (hit.collider != null)
            {
                GameObject preview = selection.GetComponent<Placement_Behavior>().placementPreview;
                GameObject raycastBullet = Instantiate(preview, hit.point, Quaternion.identity);
                raycastBullet.GetComponent<Collider>().enabled = false;
                Destroy(raycastBullet, 0.025f);
            }
        }
    }

    private void PlaceObject()
    {
        if (!active)
        {
            return;
        }

        Ray shootDirection = new(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(shootDirection, out RaycastHit hit))
        {
            if (hit.collider != null)
            {
                GameObject raycastBullet = Instantiate(selection, hit.point, Quaternion.identity);
            }
        }
    }
}
