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
    private NPCMovement npcMovement = null;

    [SerializeField]
    private int npcSpawnCount;

    [SerializeField]
    private float waveOffset;

    [SerializeField]
    private listOfPatterns flightPattern;

    [SerializeField]
    private WavePatterns wavePatterns;
    [SerializeField, Range(0, 5)]
    private float delayBetweenSpawns = 0f;
    [SerializeField, Range(1, 10)]
    private float waveSpeed = 1f;
    [SerializeField, Range(2, 10)]
    private float localOffset = 0f;

    

    //add wave timer / trigger for next wave to spawn
    //Need Flight Pattern from NPCMovement guys.

    //Accessors
    public NPCMovement GetNPCArray() { return npcMovement; }
    public WavePatterns GetWavePattern() { return wavePatterns; }
    public listOfPatterns GetFlightPattern() {  return flightPattern; }
    public float GetXOffset () { return localOffset; }
    public float GetDelayBetweenSpawns() { return delayBetweenSpawns; }
    public float GetWaveSpeed () {  return waveSpeed; }
    public int GetNPCSpawnCount () {  return npcSpawnCount; }
    public float GetWaveOffset() {  return waveOffset; }
}

//enum to describe the pattern of the spawn wave. Paterns to be defined by spawner.
public enum WavePatterns
{
    FlyingV,
    LOffset,
    ROffset,
    Sequential,
}
