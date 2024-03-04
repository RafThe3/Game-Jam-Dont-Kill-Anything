using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GrowingBehavior : MonoBehaviour
{

    [SerializeField] private float startGrowthSize = 0.01f;
    [SerializeField] private float maxGrowthSize = 0.45f;
    [SerializeField] private float growthTime = 1;
    [SerializeField] private float karmaAmount = 5;
    [SerializeField] private GameObject berries;
    [SerializeField] private bool berryBush = false;
    [SerializeField] public bool harvestable = false;

    private KarmaSystem karmaSystem;
    private float growthRate;
    private float currentGrowth;
    private float timer;
    private bool active = false;
    private bool cropActive = true;
    private void Awake()
    {
        growthRate = startGrowthSize / 2;
        maxGrowthSize = Random.Range(maxGrowthSize / 1.05f, maxGrowthSize * 1.05f);
        harvestable = false;
        gameObject.transform.localScale = new Vector3(startGrowthSize, startGrowthSize, startGrowthSize);
        karmaSystem = FindFirstObjectByType<KarmaSystem>();
    }

    private void Update()
    {
        IEnumerator growth = Growth();
        if (!active)
        {
            active = true;
            StartCoroutine(growth);
        }
        if(berryBush && !cropActive)
        {
            BerryGrowth();
        }
    }

    private void HarvestTime()
    {
        harvestable = true;
        karmaSystem.LoseKarma(karmaAmount);
    }

    private IEnumerator Growth()
    {
        currentGrowth = gameObject.transform.localScale.x;

        if (cropActive)
        {
            yield return new WaitForSeconds(growthTime);
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x + growthRate, gameObject.transform.localScale.y + growthRate, gameObject.transform.localScale.z + growthRate);
            Debug.Log("work");
            active = false;
        }
        if (currentGrowth > maxGrowthSize)
        {
            gameObject.transform.localScale = new Vector3(maxGrowthSize, maxGrowthSize, maxGrowthSize);
            cropActive = false;
        }
    }

    private void BerryGrowth()
    {
        if (transform.childCount == 0)
        {
            harvestable = false;
        }

        if (harvestable)
        {
            return;
        }

        timer += Time.deltaTime;
        bool produce = timer >= Random.Range(8, 30);
        GameObject berryObject;

        if (!cropActive || !harvestable)
        {
            if (produce)
            {
                berryObject = Instantiate(berries, transform.position, Quaternion.identity);
                berryObject.transform.parent = gameObject.transform;
                harvestable = true;
                timer = 0;
            }
        }
    }
}
