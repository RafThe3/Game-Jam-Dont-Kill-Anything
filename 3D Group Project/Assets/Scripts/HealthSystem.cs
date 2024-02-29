using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private float currentHealth;
    private LootSystem lootSystem;
    private KarmaSystem karmaSystem;

    [Header("General Settings")]
    [Min(1), SerializeField] private float maxHealth = 5;
    [Min(0), SerializeField] private float karmaValue = 5;


    private void Awake()
    {
        currentHealth = maxHealth;
        lootSystem = GetComponent<LootSystem>();
        karmaSystem = FindFirstObjectByType<KarmaSystem>();
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            EnemyDie();
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Took " + damage + " damage. New health is " + currentHealth);
    }
    public void HealDamage(float healthGained)
    {
        if (currentHealth >= maxHealth)
        {
            return;
        }

        currentHealth += healthGained;
        Debug.Log("Healed " + healthGained + " damage. New health is " + currentHealth);
    }
    public void EnemyDie()
    {
        if (!gameObject.CompareTag("Enemy"))
        {
            return;
        }

        Debug.Log("BLEHHHH.. killed " + gameObject);
        Destroy(gameObject);
        lootSystem.GetLoot();
        karmaSystem.GainKarma(karmaValue);
    }
    private void PlayerDie()
    {

    }
}
