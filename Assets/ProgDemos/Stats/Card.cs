using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Stats theStats;

    public  int strengthBonus;
    public int agilityBonus;
    public int magicBonus;

    // Start is called before the first frame update
    void Start()
    {
        theStats.UpdateStats(1, 3, 4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
