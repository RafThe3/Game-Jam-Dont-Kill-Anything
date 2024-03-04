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
        DontDestroyOnLoad(gameObject);
    }

    private void LateUpdate()
    {
        KarmaManager();
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
    public void KarmaManager()
    {
        if (karma <= 0)
        {
            karma = 0;
        }
        if(karma >= 20)
        {
            Debug.Log("monster spawn");
        }
    }
}
