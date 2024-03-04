using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [Min(0), SerializeField] private int hungerRestoreAmount = 1;

    public int GetHungerRestoreAmount()
    {
        return hungerRestoreAmount;
    }
}
