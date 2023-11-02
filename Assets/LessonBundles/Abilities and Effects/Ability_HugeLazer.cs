using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_HugeLazer : Ability_Simple
{
    [Header("Now in Ability Huge Lazer Subclass")]
    
    [SerializeField] private Vector2 boostSpeed;
    private Vector2 cachedOriginalSpeed;

    [Header(" --- Animator ---  ")]
    public Animator anim;
    private string anim_windupString = "Lazer_Windup";
    private string anim_firingString = "Lazer_Firing";
    private string anim_winddownString = "Lazer_Winddown";

    protected void Start() {
        if(anim == null) {
            anim = GetComponentInChildren<Animator>();
            if(anim == null) {
                Debug.LogWarning("Animator cannot be found by Huge Lazer ability");
            }
        }
    }

    protected override void StartWindup() {
        base.StartWindup();


        anim.SetBool(anim_windupString, true);

    }

    protected override void StartFiring() {

        base.StartFiring(); // 

        // Add Speed

        // enable physics check 
        if (abilityRigidbody != null)
            abilityRigidbody.simulated = true;

        anim.SetBool(anim_windupString, false);
        anim.SetBool(anim_firingString, true);

    }

    protected override void StartWinddown() {
        base.StartWinddown();

        // disable physics check 
        if (abilityRigidbody != null)
            abilityRigidbody.simulated = false;

        // 
        anim.SetBool(anim_firingString, false);
        anim.SetBool(anim_winddownString, true);

    }

    protected override void StartCooldown() {
        base.StartCooldown();

        anim.SetBool(anim_winddownString, false);

    }
}
