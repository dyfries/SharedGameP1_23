using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursionBomb : MonoBehaviour {
    public Ability_Recursion ability; 
    
    public GameObject toSpawnAtEnd;
    public float destroyTimer = 2f;

    public float countdownTimer = 1f;
    public int depthCounter = 3;

    public Vector2 spawnOffset = Vector2.up;

    // Start is called before the first frame update
    void Start() {
        Debug.Log("Bomb Launched with recursion depth " + depthCounter);
        Destroy(gameObject, destroyTimer); // Destroy self
    }

    // Update is called once per frame
    void Update() {
        if (depthCounter > 0) {
            countdownTimer -= Time.deltaTime;
            if (countdownTimer <= 0) {
                //Activate: 
                RecursionBomb rb = Instantiate<RecursionBomb>(ability.bombToSpawn, transform.position + (Vector3)spawnOffset, Quaternion.identity);

                // NOTE: Because of the way unity instantiated prefabs, and references self prefabs, there is an edge case here where we need to manually update
                // the child reference of the prefab so it doesn't reference the prefab that just spawned it. 
                // Long thread but the answer is in the middle (Superpigs post).
                // https://forum.unity.com/threads/reference-to-prefab-changing-to-clone-self-reference.57312/
                // rb.toSpawnRecursively = toSpawnRecursively;
                // This only works one level deep. 

                // I am getting the reference directly from ability, and have to pass the ability reference as well
                rb.ability = ability;
                rb.depthCounter = depthCounter - 1 ;
                
                
                
                // disable self so we don't spawn again
                this.enabled = false;
            } 

        } else {
            // Recursion is done
            // Timer is finished. 
            GameObject end = Instantiate<GameObject>(toSpawnAtEnd, transform.position + (Vector3)spawnOffset, Quaternion.identity);
            Destroy(end, destroyTimer);
            // disable self so we don't spawn again
            this.enabled = false;
        }
    }
}
