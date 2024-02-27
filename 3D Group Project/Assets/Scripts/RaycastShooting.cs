using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RaycastShooting : MonoBehaviour
{
    [SerializeField] private GameObject raycastVisual;
    LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    private void Shoot()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                IEnumerator coroutine = RaycastDebug();
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, hit.point);
                StartCoroutine(coroutine);
                GameObject raycastBullet;
                raycastBullet = GameObject.Instantiate(raycastVisual, hit.point, Camera.main.transform.rotation);
                Debug.Log("Hit " + hit.collider.name);
                Destroy(raycastBullet, 1);
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    private IEnumerator RaycastDebug()
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(1);
        lineRenderer.enabled = false;
    }
}
