using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LootSystem : MonoBehaviour
{
    [SerializeField] private Loot_Behavior[] lootTable;
    [SerializeField] private float lootSpread = 0;
    private GameObject lootStorage;

    private void Awake()
    {
        lootStorage = GameObject.Find("LootStorage");
    }

    public ArrayList GetLoot()
    {
        ArrayList dropList = new ArrayList();
        float drawn = Random.Range(0f, 100f);
        lootSpread = Random.Range(0f, 5f);

        foreach (Loot_Behavior loot in lootTable)
        {
            if (drawn <= loot.chance)
            {
                loot.Quantity = RandomQuantity(loot);
                dropList.Add(loot);
                for(int i = 0; i < loot.Quantity; i++)
                {
                    Instantiate(loot, gameObject.transform.position + new Vector3(lootSpread, 0, lootSpread), Quaternion.identity);
                }
            }
        }

        return dropList;
    }
    public int RandomQuantity(Loot_Behavior loot)
    {
        return Random.Range(loot.minQuantity, loot.maxQuantity);
    }

    private void DropLoot()
    {
        Debug.Log("dropped");
    }
}

