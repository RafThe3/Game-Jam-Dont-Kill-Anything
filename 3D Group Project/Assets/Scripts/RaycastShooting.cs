using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RaycastShooting : MonoBehaviour
{
    [SerializeField] private bool debug = true;
    [SerializeField] private bool meleeRaycast = true;
    [SerializeField] private bool rangedRaycast;
    [SerializeField] private bool canShoot = true;
    [SerializeField] private GameObject raycastVisual;
    [SerializeField] private GameObject collisionPoint;

    private LineRenderer lineRenderer;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Melee();
        }
    }
    private void Shoot()
    {
        if (!canShoot)
        {
            return;
        }

        Ray shootDirection = new(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(shootDirection, out RaycastHit hit))
        {
            if (hit.collider != null)
            {
                IEnumerator coroutine = RaycastDebug();
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, hit.point);
                StartCoroutine(coroutine);
                GameObject raycastBullet = Instantiate(raycastVisual, hit.point, Camera.main.transform.rotation);
                Destroy(raycastBullet, 1);
                Debug.Log("Hit " + hit.collider.name);
                if (hit.collider.CompareTag("Enemy"))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    private void Melee()
    {
        Collider[] hits = Physics.OverlapSphere(collisionPoint.transform.position, 0.5f);

        foreach (Collider hit in hits)
        {
            if (hit.transform.root != transform)
            {
                GameObject debugHit = Instantiate(raycastVisual, collisionPoint.transform.position, Camera.main.transform.rotation);
                Debug.Log(hit.name);
                Destroy(debugHit, 1);
            }
            if (hit.GetComponent<Collider>().CompareTag("Enemy"))
            {
                hit.gameObject.GetComponent<HealthSystem>().TakeDamage(1);
            }
        }
    }

    private IEnumerator RaycastDebug()
    {
        if (!debug)
        {
            yield return null;
        }
        else
        {
            if (!lineRenderer.enabled)
            {
                lineRenderer.enabled = true;
            }
            else
            {
                lineRenderer.enabled = false;
                lineRenderer.enabled = true;
            }
        }
    }
}
