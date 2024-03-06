using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private bool active = false;
    [SerializeField] private Placement_Behavior[] selection;

    private int selectedChoice;
    private int choices;

    void Start()
    {
        choices = selection.Length;
        selectedChoice = 0;
    }
    void Update()
    {
        PlaceObjectHologram();
        if ((Input.GetKeyDown(KeyCode.E)))
        {
            PlaceObject();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(active)
            {
                active = false;
            }
            else
            {
                active = true;
            }
        }
        else if(Input.GetKeyDown(KeyCode.Z))
        {
            CycleSelection();
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
                GameObject preview = selection[selectedChoice].gameObject.GetComponent<Placement_Behavior>().placementPreview;
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
                GameObject raycastBullet = Instantiate(selection[selectedChoice].gameObject, hit.point, Quaternion.identity);
            }
        }
    }
    private void CycleSelection()
    {
        if (selectedChoice > choices)
        {
            selectedChoice = 0;
        }
        selectedChoice++;
    }
}
