using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Min(1), SerializeField] private float maxHealth = 5;
    private float currentHealth;
    private LootSystem lootSystem;

    private void Awake()
    {
        currentHealth = maxHealth;
        lootSystem = GetComponent<LootSystem>();
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
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

    public void Die()
    {
        Debug.Log("BLEHHHH.. killed " + gameObject);
        Destroy(gameObject);
        lootSystem.GetLoot();
    }
}
