using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private bool isInvincible = false;
    [Min(0), SerializeField] private int maxHealth = 100;
    [Min(0), SerializeField] private int healAmount = 10;
    [Min(0), SerializeField] private int startingHealthPacks = 1;
    [Min(0), SerializeField] private int maxHealthPacks = 10;
    [Min(0), SerializeField] private int healthPackGainInterval = 1;
    [Min(0), SerializeField] private float healDelay = 1;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image healthBarFillArea;
    [SerializeField] private Color defaultHealthColor = Color.green;
    [SerializeField] private Color lowHealthColor = Color.red;
    [SerializeField] private TextMeshProUGUI healthPacksText;
    [SerializeField] private AudioClip healSFX, hurtSFX;

    [Header("Hunger")]
    [SerializeField] private bool canEat = true;
    [Min(0), SerializeField] private int maxHunger = 100;
    [Min(0), SerializeField] private int hungerDecreaseAmount = 1;
    [Min(0), SerializeField] private float hungerDecreaseInterval = 1;
    [Min(0), SerializeField] private int healthDecreaseAmount = 1;
    [Min(0), SerializeField] private int hungerRestoreAmount = 1;
    [Min(0), SerializeField] private float healthDecreaseInterval = 1;
    [Min(0), SerializeField] private float eatInterval = 1;
    [SerializeField] private Slider hungerBar;
    [SerializeField] private Image hungerBarFillArea;
    [SerializeField] private Color defaultHungerBarColor = Color.yellow;
    [SerializeField] private Color lowHungerColor = Color.red;

    [Header("Pickup")]
    [SerializeField] private bool canPickup = true;
    [SerializeField] private GameObject hand;
    [Min(0), SerializeField] private float pickupDistance = 1;
    [SerializeField] private TextMeshProUGUI interactText;

    //Internal Variables

    //Health
    private int healthPacks = 0, currentHealth = 0, currentHunger = 0;
    private AudioSource audioSource;
    private bool isHealing, isEating;
    private float hungerDecreaseTimer = 0, healthDecreaseTimer = 0;
    private float healthPackGainTimer = 0;

    //Inventory
    private GameObject currentObjectInHand;
    private List<GameObject> inventory = new();

    //Other
    private int numFood;

    private void Awake()
    {
        Time.timeScale = 1;
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = healthBar.maxValue;
        currentHunger = maxHunger;
        hungerBar.maxValue = maxHunger;
        hungerBar.value = hungerBar.maxValue;
        healthPacks = startingHealthPacks;
        if (maxHealthPacks == 0)
        {
            maxHealthPacks = startingHealthPacks;
        }
    }

    private void Update()
    {
        FixBugs();
        UpdateUI();
        DecreaseHunger(hungerDecreaseAmount);
        bool canGainHealthPack;
        canGainHealthPack = healthPacks < maxHealthPacks;

        if (canGainHealthPack)
        {
            healthPackGainTimer += Time.deltaTime;
        }
        else
        {
            healthPackGainTimer = 0;
        }

        if (healthPacks < maxHealthPacks && healthPackGainTimer >= healthPackGainInterval)
        {
            healthPacks++;
            healthPackGainTimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            FindObjectOfType<HealthSystem>().TakeDamage(10);
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.Q) && !isHealing)
        {
            StartCoroutine(Heal(healAmount));
        }

        if (currentHunger <= 0)
        {
            AutoTakeDamage(healthDecreaseAmount);
        }

        Ray pickupDirection = new(Camera.main.transform.position, Camera.main.transform.forward);
        bool isNearObject = Physics.Raycast(pickupDirection, out RaycastHit hit, pickupDistance);

        interactText.enabled = isNearObject && hit.collider.CompareTag("Item");

        if (isNearObject && canPickup)
        {
            bool isItem = hit.collider.CompareTag("Item");

            if (isItem)
            {
                interactText.text = $"Press {KeyCode.E} to eat {hit.collider.name}";
            }

            if (Input.GetKeyDown(KeyCode.E) && isItem && currentHunger < maxHunger)
            {
                StartCoroutine(Eat(hungerRestoreAmount));
                Destroy(hit.collider.gameObject);
            }
        }
    }

    //Methods

    //Health
    private void FixBugs()
    {
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (currentHunger < 0)
        {
            currentHunger = 0;
        }

        if (currentHunger > maxHealth)
        {
            currentHunger = maxHealth;
        }
    }

    public void TakeDamage(int health)
    {
        if (isInvincible)
        {
            return;
        }

        if (currentHealth > 0)
        {
            currentHealth -= health;
            if (hurtSFX != null)
            {
                audioSource.PlayOneShot(hurtSFX);
            }
        }
    }

    public void AutoTakeDamage(int health)
    {
        if (healthDecreaseTimer >= healthDecreaseInterval)
        {
            currentHealth -= health;
            healthDecreaseTimer = 0;
        }
    }

    public IEnumerator Heal(int health)
    {
        isHealing = true;

        if (currentHealth < maxHealth && healthPacks > 0)
        {
            currentHealth += health;
            healthPacks--;
            if (healSFX != null)
            {
                audioSource.PlayOneShot(healSFX);
            }
        }

        yield return new WaitForSeconds(healDelay);
        isHealing = false;
    }

    public IEnumerator Eat(int amount)
    {
        if (currentHunger >= maxHunger && isEating)
        {
            yield break;
        }

        isEating = true;

        if (currentHunger < maxHunger)
        {
            currentHunger += amount;
            numFood--;
            Debug.Log("Eating");
        }

        yield return new WaitForSeconds(eatInterval);

        isEating = false;
    }

    private void DecreaseHunger(int amount)
    {
        if (hungerDecreaseTimer >= hungerDecreaseInterval)
        {
            currentHunger -= amount;
            hungerDecreaseTimer = 0;
        }
    }

    private void Die()
    {
        Cursor.lockState = CursorLockMode.None;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }

    public void AddHealthPack()
    {
        if (healthPacks < maxHealthPacks)
        {
            healthPacks++;
        }
    }

    //Other
    private void PickupObject(RaycastHit hit)
    {
        Quaternion resetRotation = new(0, 0, 0, 0);
        GameObject clone = Instantiate(hit.collider.gameObject, hand.transform.position, resetRotation, hand.transform);
        clone.GetComponent<Rigidbody>().isKinematic = true;
        clone.GetComponent<Collider>().isTrigger = true;
        currentObjectInHand = clone;
        Debug.Log(clone.layer);
        if (clone.layer == 6)
        {
            numFood++;
        }
        inventory.Add(clone);
        Destroy(hit.collider.gameObject);
    }

    //UI
    private void UpdateUI()
    {
        healthPacksText.text = $"Health Packs: {healthPacks}";
        healthBar.value = currentHealth;
        hungerBar.value = currentHunger;
        healthBarFillArea.color = currentHealth > 25 ? defaultHealthColor : lowHealthColor;
        hungerBarFillArea.color = currentHunger > 25 ? defaultHungerBarColor : lowHungerColor;
        switch (currentHunger)
        {
            case <= 0:
                hungerDecreaseTimer = 0;
                healthDecreaseTimer += Time.deltaTime;
                break;
            default:
                healthDecreaseTimer = 0;
                hungerDecreaseTimer += Time.deltaTime;
                break;
        }
    }

    private void OnDrawGizmos() => Gizmos.DrawWireSphere(transform.position, pickupDistance);
}
