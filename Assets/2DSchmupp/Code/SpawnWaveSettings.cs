using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveSettings", menuName = "ScriptableObjects/WaveSettings", order = 1)]
public class SpawnWaveSettings : ScriptableObject
{
    //Variables
    [SerializeField]
    private BaseNPC[] NPCArray = null;
    [SerializeField]
    private WavePatterns wavePatterns;
    [SerializeField]
    private float delayBetweenSpawns = 0f;
    [SerializeField]
    private float horizontalSpaceBetweenNPCS = 0f;
    //Need Flight Pattern from NPC guys.

    //Accessors
    public BaseNPC[] GetNPCArray() { return NPCArray; }
    public WavePatterns GetWavePattern() { return wavePatterns; }
    public float GetHorizontalSpacing () { return horizontalSpaceBetweenNPCS; }
    public float GetDelayBetweenSpawns() { return delayBetweenSpawns; }
}

// May need to move into own script at some point.
public enum WavePatterns
{
    V,
    LOffset,
    ROffset,
    Sequential,
}
