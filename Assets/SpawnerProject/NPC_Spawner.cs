using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// This first draft will simply spawn one unit per time, randomly in a line. 
// Will expand it later. 
public class NPC_Spawner : MonoBehaviour
{
    // Currently using generic GameObject so we can spawn anything. 
    // If we needed to communicate with the NPC, we would need to chose the correct type. 
    [SerializeField] private SpawnWaveSettings[] _settingsArray;

    public GameObject prefabToSpawn;

    public float delayBetweenWaves = 5f;

    // Local counters.
    private float waveIntervalTimer = 0f;
    private float npcIntervalTimer = 0f;

    private int _spawnIndex;
    private int _waveIndex = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        _waveIndex = 0;
        _spawnIndex = 0;

        waveIntervalTimer = 0;
        npcIntervalTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float delayBetweenSpawns = _settingsArray[_waveIndex].GetDelayBetweenSpawns();

        //if (waveIntervalTimer >= _settingsArray.GetDelayBetweenWaves())
        if (waveIntervalTimer >= delayBetweenWaves)
        {
            // Start spawning wave.

            if (npcIntervalTimer >= delayBetweenSpawns)
            {
                // Spawn NPC here.

                if(prefabToSpawn != null)
                {
                    SpawnNPC();                    
                    Debug.Log(_spawnIndex);
                    Debug.Log(_waveIndex);
                    Debug.Log(delayBetweenSpawns);
                }
                else
                {
                    Debug.LogError(gameObject.name + " needs NPC prefab assigned in inspector.");
                }

                

                // If spawn index points to the last NPC in the array, reset.                
                if (_spawnIndex >= _settingsArray[_waveIndex].GetNPCSpawnCount())
                {
                    _spawnIndex = 0;

                    // Reset wave index.
                    _waveIndex = (_waveIndex + 1) % _settingsArray.Length;
                    waveIntervalTimer = 0;
                }
                else
                {
                    _spawnIndex++;
                    npcIntervalTimer = 0;
                }
            }
            else
            {
                npcIntervalTimer += Time.deltaTime;
            }            
        }
        else
        {
            waveIntervalTimer += Time.deltaTime;
        }

        /*if(!spawnLimit || amountSpawnedSoFar < totalToSpawn) {
            currentTimer += Time.deltaTime;
            if (currentTimer > spawnTimer) {
                SpawnNPC();
                currentTimer = 0;
                amountSpawnedSoFar++;
            }
        }*/        
    }

    public void SpawnNPC() {
        // chose random location

        float xOffset = _spawnIndex * _settingsArray[_waveIndex].GetXOffset();
        Vector3 spawnLocation = transform.position;


        // Create instance
        // use the current position as the origin, Z is always 0 for 2D
        if (_settingsArray[_waveIndex].GetWavePattern() == WavePatterns.LOffset)
        {
            spawnLocation.x -= xOffset;
        }
        else if (_settingsArray[_waveIndex].GetWavePattern() == WavePatterns.ROffset)
        {
            spawnLocation.x += xOffset;
        }
        else if (_settingsArray[_waveIndex].GetWavePattern() == WavePatterns.FlyingV)
        {
            if (_spawnIndex > 0)
            {
                Vector3 flyingVSpawn = new Vector3 (transform.position.x - xOffset, transform.position.y, transform.position.z);
                GameObject leftVNPC = Instantiate(prefabToSpawn, flyingVSpawn, Quaternion.identity, transform);
                leftVNPC.GetComponent<NPCMovement>().pattern = _settingsArray[_waveIndex].GetFlightPattern();
                spawnLocation.x += xOffset;
            }
        }


        // Lots of different options here: 
        // Prefab to spawn in the reference to the Prefab - Use the prefab on disk on in the scene, so you can set this in the Spawner prefab as well. 
        // location already includes our local position, 
        // Quaterinion.identity is a fancy way of saying no rotation. It is a zero'd Quaternion (we will talk about later). 
        // Settings this spawner as the parent, don't do this if the spawner moves or all child objects will move with it. 


        GameObject go = Instantiate(prefabToSpawn, spawnLocation ,Quaternion.identity, transform);

        go.GetComponent<NPCMovement>().pattern = _settingsArray[_waveIndex].GetFlightPattern();        


        // [ ] maybe we track it later but for now we won't keep a reference (it gets tricky when dealing with destroying the objects). 

    }    
}