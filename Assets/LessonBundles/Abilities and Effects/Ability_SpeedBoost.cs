using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_SpeedBoost : Ability_Simple {

    [Header("Now in Ability Speedboost Subclass")]
    public PlayerController pc;
    [SerializeField] private Vector2 boostSpeed;
    private Vector2 cachedOriginalSpeed;

    protected void Start() {
        pc = GetComponentInParent<PlayerController>();
        if(pc == null) {
            Debug.LogWarning("No Parent Controller Found");
            enabled = false;
        } else {
            cachedOriginalSpeed = pc.moveForce;
            boostSpeed = pc.moveForce * 2f;
        }

    }

    protected override void StartFiring() {

        base.StartFiring(); // 

        // Add Speed

        pc.moveForce = boostSpeed;

    }

    protected override void StartWinddown() {
        base.StartWinddown();
        pc.moveForce = cachedOriginalSpeed;
    }



}
