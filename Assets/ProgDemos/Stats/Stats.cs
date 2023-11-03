using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public int strength = 4;
    public int agility = 3;
    public int magic = 2;

    public void UpdateStats(int s, int a, int m) {
        strength += s;
        agility += a;
        magic += m;
    }
}
