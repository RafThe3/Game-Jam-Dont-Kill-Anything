using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Health")]
    [Min(0), SerializeField] private int maxHealth = 100;
    [Min(0), SerializeField] private int healAmount = 10;
    [Min(0), SerializeField] private int startingHealthPacks = 1;
    [Min(0), SerializeField] private int maxHealthPacks = 10;
    [Min(0), SerializeField] private float healDelay = 1;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image healthBarFillArea;
    [SerializeField] private Color defaultHealthColor = Color.green;
    [SerializeField] private Color lowHealthColor = Color.red;
    [SerializeField] private TextMeshProUGUI healthPacksText;
    [SerializeField] private AudioClip healSFX, hurtSFX;

    [Header("Hunger")]
    [Min(0), SerializeField] private int maxHunger = 100;
    [Min(0), SerializeField] private int hungerDecreaseAmount = 1;
    [Min(0), SerializeField] private float hungerDecreaseInterval = 1;
    [Min(0), SerializeField] private int healthDecreaseAmount = 1;
    [Min(0), SerializeField] private float healthDecreaseInterval = 1;
    [Min(0), SerializeField] private float eatInterval = 1;
    [SerializeField] private Slider hungerBar;
    [SerializeField] private Image hungerBarFillArea;
    [SerializeField] private Color defaultHungerBarColor = Color.yellow;
    [SerializeField] private Color lowHungerColor = Color.red;

    //Internal Variables
    private int healthPacks = 0, currentHealth = 0, currentHunger = 0;
    private AudioSource audioSource;
    private bool isHealing, isEating;
    private float hungerDecreaseTimer = 0, healthDecreaseTimer = 0;

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
        FixHealthBugs();
        UpdateUI();
        DecreaseHunger(hungerDecreaseAmount);

        if (currentHealth <= 0)
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.E) && !isHealing)
        {
            StartCoroutine(Heal(healAmount));
        }

        if (currentHunger <= 0)
        {
            AutoTakeDamage(healthDecreaseAmount);
        }

        //test
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.F) && !isEating)
        {
            StartCoroutine(Eat(10));
        }
    }

    //Methods

    private void FixHealthBugs()
    {
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void TakeDamage(int health)
    {
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
        isEating = true;

        if (currentHunger < maxHunger)
        {
            currentHunger += amount;
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
        GetComponent<CharacterController>().Move(Vector3.zero);
    }

    public void AddHealthPack()
    {
        if (healthPacks < maxHealthPacks)
        {
            healthPacks++;
        }
    }

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
}
