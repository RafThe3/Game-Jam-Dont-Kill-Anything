using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Loot_Behavior : MonoBehaviour
{
    public Item item;
    public float chance;
    public int minQuantity;
    public int maxQuantity;

    public bool isFood = false;
    public bool isMaterial = false;
    public bool isAmmo = false;

    public int Quantity { get; set; }
}
