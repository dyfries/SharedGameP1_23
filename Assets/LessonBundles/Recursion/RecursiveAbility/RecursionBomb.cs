using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// An example of recursion with Gameobjects/Prefabs being spawned. 
// Note: Because of the weird nature of prefabs in unity, I couldn't simply keep a reference to a prefab in them, and had to
// work around by storing the prefab in the ability and retreiving it at runtime. 
public class RecursionBomb : MonoBehaviour {
    public Ability_Recursion ability; 
    
    public GameObject toSpawnAtEnd;
    public float destroyTimer = 2f;

    public float countdownTimer = 1f;
    public int depthCounter = 3;

    public Vector2 spawnOffsetLeft = new Vector2(1f, 1f);
    public Vector2 spawnOffsetRight = new Vector2(-1f, 1f);

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
                Debug.DrawRay(transform.position, spawnOffsetLeft, Color.magenta);
                Debug.DrawRay(transform.position, spawnOffsetRight, Color.blue);

                RecursionBomb rb = Instantiate<RecursionBomb>(ability.bombToSpawn, transform.position + (Vector3)spawnOffsetLeft, Quaternion.identity);
                RecursionBomb rb2 = Instantiate<RecursionBomb>(ability.bombToSpawn, transform.position + (Vector3)spawnOffsetRight, Quaternion.identity);

                // NOTE: Because of the way unity instantiated prefabs, and references self prefabs, there is an edge case here where we need to manually update
                // the child reference of the prefab so it doesn't reference the prefab that just spawned it. 
                // Long thread but the answer is in the middle (Superpigs post).
                // https://forum.unity.com/threads/reference-to-prefab-changing-to-clone-self-reference.57312/
                // rb.toSpawnRecursively = toSpawnRecursively;
                // This only works one level deep. 

                // I am getting the reference directly from ability, and have to pass the ability reference as well
                rb.ability = ability;
                rb.depthCounter = depthCounter - 1 ;

                rb2.ability = ability;
                rb2.depthCounter = depthCounter - 1;

                // disable self so we don't spawn again
                this.enabled = false;
            } 

        } else {
            // Recursion is done

            // Add a random offset at the end so you can see where they are overlapping. 
            Vector2 randomOffset1 = Random.insideUnitCircle;
            Vector2 randomOffset2 = Random.insideUnitCircle;

            // Timer is finished. 
            GameObject end = Instantiate<GameObject>(toSpawnAtEnd, transform.position + (Vector3)spawnOffsetLeft + (Vector3)randomOffset1, Quaternion.identity);
            Destroy(end, destroyTimer);

            GameObject end2 = Instantiate<GameObject>(toSpawnAtEnd, transform.position + (Vector3)spawnOffsetRight + (Vector3)randomOffset2, Quaternion.identity);
            Destroy(end2, destroyTimer);

            // disable self so we don't spawn again
            this.enabled = false;
        }
    }
}
