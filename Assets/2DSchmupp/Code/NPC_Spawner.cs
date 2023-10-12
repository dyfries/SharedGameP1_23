using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This first draft will simply spawn one unit per time, randomly in a line. 
// Will expand it later. 
public class NPC_Spawner : MonoBehaviour
{
    // Currently using generic GameObject so we can spawn anything. 
    // If we needed to communicate with the NPC, we would need to chose the correct type. 
    public GameObject prefabToSpawn;

    [Header("Area of effect")]
    public float spawnableWidth = 10f;

    [Header("Timer Settings")]
    private float currentTimer;
    public float spawnTimer;

    [Header("Amount to spawn")]
    public bool spawnLimit = false;
    private int amountSpawnedSoFar = 0;
    public int totalToSpawn = 10;

    [Header("Initialization Settings")]
    public bool spawnOnStart = true;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn one at start. 
        if (spawnOnStart) {
            SpawnNPC();        
        }
        currentTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!spawnLimit || amountSpawnedSoFar < totalToSpawn) {
            currentTimer += Time.deltaTime;
            if (currentTimer > spawnTimer) {
                SpawnNPC();
                currentTimer = 0;
                amountSpawnedSoFar++;
            }
        }
        
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
