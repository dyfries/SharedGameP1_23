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
    [SerializeField, Header("Array Of Non-Player Characters")]
    private Npc[] NPCArray = null;
    [SerializeField, Header("Wave Pattern")]
    private WavePatterns wavePatterns;
    [SerializeField, Range(0, 5)]
    private float delayBetweenSpawns = 0f;
    [SerializeField, Range(1, 10)]
    private float waveSpeed = 1f;
    [SerializeField, Range(2, 10), Header("Horizontal Distance Between Non-Player Characters")]
    private float xOffset = 0f;

    //add wave timer / trigger for next wave to spawn
    //Need Flight Pattern from NPC guys.

    //Accessors
    public Npc[] GetNPCArray() { return NPCArray; }
    public WavePatterns GetWavePattern() { return wavePatterns; }
    public float GetXOffset () { return xOffset; }
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
