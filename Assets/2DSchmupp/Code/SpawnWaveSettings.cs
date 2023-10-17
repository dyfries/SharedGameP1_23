using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//right click in inspector, go to create -> ScriptableObjects -> WaveSettings to create a new object instance
//To access in another script, have a SpawnWaveSettings type variable
//Ex: public SpawnWaveSettings spawnWaveSettings;
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
    [SerializeField]
    private float waveSpeed = 1f;

    //add wave timer / trigger for next wave to spawn

    //Need Flight Pattern from NPC guys.

    //Accessors
    public BaseNPC[] GetNPCArray() { return NPCArray; }
    public WavePatterns GetWavePattern() { return wavePatterns; }
    public float GetHorizontalSpacing () { return horizontalSpaceBetweenNPCS; }
    public float GetDelayBetweenSpawns() { return delayBetweenSpawns; }
    public float GetWaveSpeed () {  return waveSpeed; }
}

//enum to describe the pattern of the spawn wave. Paterns to be defined by spawner.
public enum WavePatterns
{
    FlyingV,
    LOffset,
    ROffset,
    Sequential,
}
