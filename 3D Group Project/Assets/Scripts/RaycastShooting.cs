using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RaycastShooting : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool debug = true;
    [SerializeField] private GameObject raycastVisual;
    [SerializeField] private GameObject collisionPoint;

    [Header("Weapon Variables")]
    [SerializeField] private bool meleeRaycast = true;
    [SerializeField] private bool rangedRaycast = false;
    [SerializeField] private bool canAttack = true;

    [Header("Weapon Settings")]
    [SerializeField] private float meleeCooldown = 1;
    [SerializeField] private float rangedCooldown = 0.5f;
    [Min(0.1f), SerializeField] private float meleeRadius = 0.5f;

    private LineRenderer lineRenderer;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        collisionPoint.transform.localScale = new Vector3(meleeRadius, meleeRadius, meleeRadius);
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
        if (!canAttack)
        {
            return;
        }

        Ray shootDirection = new(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(shootDirection, out RaycastHit hit))
        {
            if (hit.collider != null)
            {
                IEnumerator debugRaycast = RaycastDebug();
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, hit.point);
                StartCoroutine(debugRaycast);
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
        IEnumerator cooldown = WeaponCooldown(meleeCooldown);
        Debug.Log("test");

        if (!canAttack)
        {
            return;
        }

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
                StartCoroutine(cooldown);
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

    private IEnumerator WeaponCooldown(float seconds)
    {
        canAttack = false;
        yield return new WaitForSeconds(seconds);
        canAttack = true;
    }

    // for editing in-game
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(collisionPoint.transform.position, meleeRadius);
        Gizmos.color = Color.red;
    }
}
