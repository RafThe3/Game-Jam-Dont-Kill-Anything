using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder;

public class RaycastShooting : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool debug = true;
    [SerializeField] private bool healyShot = false;
    [SerializeField] private GameObject raycastVisual;
    [SerializeField] private GameObject collisionPoint;

    [Header("Weapon Variables")]
    [SerializeField] private bool meleeRaycast = true;
    [SerializeField] private bool rangedRaycast = false;
    [SerializeField] private bool canAttack = true;
    [SerializeField] private bool canInteract = true;

    [Header("Weapon Settings")]
    [SerializeField] private float meleeCooldown = 1;
    [Min(0), SerializeField] private float meleeDamage = 1;
    [Min(0.1f), SerializeField] private float meleeRadius = 0.5f;
    [SerializeField] private float rangedCooldown = 0.5f;
    [Min(0), SerializeField] private float rangedDamage = 0.5f;
    [Min(0.1f), SerializeField] private float rangedRange = 5f;

    [Header("Other")]
    [SerializeField] private TextMeshProUGUI pickupText;

    private LineRenderer lineRenderer;
    public bool canPickup = false;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        pickupText.GetComponent<TextMeshProUGUI>().enabled = false;
        collisionPoint.transform.localScale = new Vector3(meleeRadius * 2, meleeRadius * 2, meleeRadius * 2);
    }
    private void Update()
    {
        IEnumerator raycast = InteractionRaycast();

        if (Input.GetButtonDown("Fire1"))
        {
            Melee();
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            Shoot();
        }

        StartCoroutine(raycast);
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
                IEnumerator cooldown = WeaponCooldown(rangedCooldown);

                StartCoroutine(debugRaycast);
                GameObject raycastBullet = Instantiate(raycastVisual, hit.point, Camera.main.transform.rotation);
                Destroy(raycastBullet, 1);
                Debug.Log("Hit " + hit.collider.name);
                if (hit.collider.CompareTag("Enemy"))
                {
                    if(healyShot)
                    {
                        hit.collider.gameObject.GetComponent<HealthSystem>().HealDamage(rangedDamage);
                    }
                    else
                    {
                        hit.collider.gameObject.GetComponent<HealthSystem>().TakeDamage(rangedDamage);
                    }
                }
                StartCoroutine(cooldown);
            }
        }
    }
    private void Melee()
    {
        Collider[] hits = Physics.OverlapSphere(collisionPoint.transform.position, meleeRadius);
        IEnumerator cooldown = WeaponCooldown(meleeCooldown);
        Debug.Log("test");

        if (!canAttack)
        {
            return;
        }

        foreach (Collider hit in hits)
        {
            if (!hit.gameObject.CompareTag("Player") || !hit.gameObject.CompareTag("Terrain"))
            {
                if(debug)
                {
                    GameObject debugHit = Instantiate(raycastVisual, collisionPoint.transform.position, Camera.main.transform.rotation);
                    Debug.Log(hit.name);
                    Destroy(debugHit, 1);
                }
            }
            if (hit.GetComponent<Collider>().CompareTag("Enemy"))
            {
                hit.gameObject.GetComponent<HealthSystem>().TakeDamage(meleeDamage);
            }
        }
        StartCoroutine(cooldown);
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
    private IEnumerator InteractionRaycast()
    {
        Ray shootDirection = new(Camera.main.transform.position, Camera.main.transform.forward);

        if (!canInteract)
        {
            yield return null;
        }

        if (Physics.Raycast(shootDirection, out RaycastHit hit))
        {

            if (hit.collider != null)
            {
                IEnumerator debugRaycast = RaycastDebug();
                if (debug)
                {
                    StartCoroutine(debugRaycast);
                    GameObject raycastBullet = Instantiate(raycastVisual, hit.point, Camera.main.transform.rotation);
                    Destroy(raycastBullet, 1);
                    Debug.Log("Hit " + hit.collider.name);
                }
                if (hit.collider.CompareTag("Item"))
                {
                    pickupText.GetComponent<TextMeshProUGUI>().enabled = true;
                    Debug.Log("Can pick this up.");
                    canPickup = true;
                    pickupText.text = "Pick up " + hit.collider.name;
                }
                else
                {
                    pickupText.GetComponent<TextMeshProUGUI>().enabled = false;
                    canPickup = false;
                }
            }
        }
    }

    // Input system actions. Feel free to move if necessary
    void OnInteract(InputValue value)
    {
        if(value.isPressed)
        {
            Debug.Log("test");
            Ray shootDirection = new(Camera.main.transform.position, Camera.main.transform.forward);

            if (!canInteract)
            {
                Debug.Log("brapp");
                return;
            }

            if (Physics.Raycast(shootDirection, out RaycastHit hit))
            {
                if (hit.collider != null)
                {
                    Debug.Log("ssss");
                    IEnumerator debugRaycast = RaycastDebug();
                    if (debug)
                    {
                        StartCoroutine(debugRaycast);
                        GameObject raycastBullet = Instantiate(raycastVisual, hit.point, Camera.main.transform.rotation);
                        Destroy(raycastBullet, 1);
                        Debug.Log("Hit " + hit.collider.name);
                    }
                    if (value.isPressed && canPickup)
                    {
                        Debug.Log("Pik up");
                        Destroy(hit.transform.gameObject);
                    }
                }
            }
        }
    }

    // for editing in-game
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(collisionPoint.transform.position, meleeRadius);
        Gizmos.color = Color.green;
    }
}
