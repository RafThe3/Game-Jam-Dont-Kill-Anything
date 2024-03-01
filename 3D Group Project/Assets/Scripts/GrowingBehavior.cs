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
    [SerializeField] public bool harvestable = false;


    private float growthRate;
    private float currentGrowth;
    private float maxGrowth;
    private bool active = false;
    private bool cropActive = true;
    private void Awake()
    {
        growthRate = startGrowthSize / 2;
        harvestable = false;
        gameObject.transform.localScale = new Vector3 (startGrowthSize, startGrowthSize, startGrowthSize);
    }

    private void Start()
    {
    }

    private void Update()
    {
        IEnumerator growth = Growth();
        if (!active)
        {
            active = true;
            StartCoroutine(growth);
        }

    }

    private void GrowthCycle()
    {

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

}
