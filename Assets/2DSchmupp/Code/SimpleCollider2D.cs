using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Add Events
using UnityEngine.Events;


public class SimpleDeathCollider2D : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnTriggerEnter_Event = new UnityEvent();
    public UnityEvent OnCollisionEnter_Event = new UnityEvent();

    [Header("Destruction")]
    public bool destroyOnCollision = true;
    public bool destroyOnTrigger = true;

    [Header("Spawn on Collision")]
    public bool spawnOnCollision = true;
    public bool spawnOnTrigger = true;
    public GameObject objectToSpawn;

    public bool DEBUG_MODE = false;

    private void OnCollisionEnter2D(Collision2D collision) {
        OnCollisionEnter_Event.Invoke();

        if (destroyOnCollision) {
            Destroy(gameObject);
        }

        if (spawnOnCollision) {
            SpawnOnCollision();
        }

        if (DEBUG_MODE) {
            Debug.Log("Object " + gameObject.name + " collided with " + collision.gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        OnTriggerEnter_Event.Invoke();

        if (destroyOnTrigger) {
            Destroy(gameObject);
        }

        if (spawnOnTrigger) {
            SpawnOnCollision();
        }

        if (DEBUG_MODE) {
            Debug.Log("Object " + gameObject.name + " hit the trigger of " + collision.gameObject.name);
        }
    }

    private void SpawnOnCollision() {
        // Coding defensively so if you forget the prefab reference it reminds you and doesn't crash. 
        if (objectToSpawn) {
            Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        } else {
            Debug.LogWarning("You are trying to spawn an object from SimpleCollider on " + gameObject.name + " but your prefab reference is null");
        }
    }
}
