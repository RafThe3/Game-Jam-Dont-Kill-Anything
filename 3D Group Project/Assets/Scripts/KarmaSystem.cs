using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class KarmaSystem : MonoBehaviour
{
    [Min(0), SerializeField] private float karma = 0;

    private void Awake()
    {
        karma = 0;
    }

    public void LoseKarma(float karmaLost)
    {
        karma -= karmaLost;
        Debug.Log("Lost karma. New karma is " + karma);
    }
    public void GainKarma(float karmaGained)
    {
        karma += karmaGained;
        Debug.Log("Gained karma. New karma is " + karma);
    }
}
