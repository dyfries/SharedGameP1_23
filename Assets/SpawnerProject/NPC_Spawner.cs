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


    // TODO: REPLACE WITH VAR IN WAVESETTINGS.
    [Header("Area of effect")]
    public float spawnableWidth = 10f;

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

                SpawnNPC();

                print("SPAWNING NPC");

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
        float xPos = Random.Range(-spawnableWidth, spawnableWidth);
        // Create instance
        // use the current position as the origin, Z is always 0 for 2D
        Vector3 spawnLocation = new Vector3(transform.position.x + xPos, transform.position.y, 0);

        // Lots of different options here: 
        // Prefab to spawn in the reference to the Prefab - Use the prefab on disk on in the scene, so you can set this in the Spawner prefab as well. 
        // location already includes our local position, 
        // Quaterinion.identity is a fancy way of saying no rotation. It is a zero'd Quaternion (we will talk about later). 
        // Settings this spawner as the parent, don't do this if the spawner moves or all child objects will move with it. 
        GameObject go = Instantiate(prefabToSpawn, spawnLocation,Quaternion.identity, transform);
        // not using the GO here just demonstrating that it is being created. 

        // [ ] maybe we track it later but for now we won't keep a reference (it gets tricky when dealing with destroying the objects). 

    }    
}