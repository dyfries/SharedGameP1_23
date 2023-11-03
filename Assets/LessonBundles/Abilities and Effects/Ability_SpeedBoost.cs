using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_SpeedBoost : Ability_Simple {

    [Header("Now in Ability Speedboost Subclass")]
    public PlayerController pc;
    [Header("Variables to be set")]
    [SerializeField] private Vector2 boostSpeed = new Vector2(60f,40f);
    private Vector2 cachedOriginalSpeed;

    [Header("Art")]
    [SerializeField] private List<GameObject> regularEngineEffects;
    [SerializeField] private List<GameObject> boostedEngineEffects;


    protected void Start() {
        pc = GetComponentInParent<PlayerController>();
        if(pc == null) {
            Debug.LogWarning("No Parent Controller Found");
            enabled = false;
        } else {
            cachedOriginalSpeed = pc.moveForce;
        }

    }

    protected override void StartFiring() {

        base.StartFiring(); // 

        // Add Speed
        pc.moveForce = boostSpeed;

        // Art Effects
        foreach (GameObject effects in regularEngineEffects) {
            effects.SetActive(false);
        }

        foreach(GameObject effects in boostedEngineEffects) {
            effects.SetActive(true);
        }

        // [ ] Increase sound pitch and volume here. 

    }

    protected override void StartWinddown() {
        base.StartWinddown();
        pc.moveForce = cachedOriginalSpeed;

        // Art Effects
        foreach (GameObject effects in regularEngineEffects) {
            effects.SetActive(true);
        }

        foreach (GameObject effects in boostedEngineEffects) {
            effects.SetActive(false);
        }
    }



}
