using JetBrains.Annotations; // Not sure where some of these are coming from. 
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;

// Using enums is useful here since it shares the properties we want our
// state machine to have, namely we can always only be in one state at any
// given moment. 
// Ready - Available to fire if activated
// Windup - Has started but hasn't fired yet (eg not physics)
// Firing - is damaging/healing/shielding/whatever
// Winddown - Has finished firing and cleaning up the effects, no physics. 
// Cooldown - Done but not ready to fire again yet. 
public enum StageOfAbility {ready, windup, firing, winddown, cooldown };

public class Ability_Simple : MonoBehaviour {

    [Header("Timers")]
    // I'm just sharing this for all the current timers. 
    private float currentTimer = 0;

    [Header("Timers for the stages of Ability activation")]
    public float activatedAbility_WindupTimer = 1f;
    public float activatedAbility_FiringTimer = 2f;
    public float activatedAbility_WinddownTimer = 1f;

    [Header("Cooldown Timer for the whole ability")]
    public float activatedAbility_CooldownTimer = 3f;


    // Art 
    // Could be sprites, or animation states. 
    // The animator needs its state to be set to floats and bools so 
    // having the logic in the script tightly control that is a good idea. 
    // You can have some of the "logic" in the animation graph itself
    // (in terms of blend trees or branching)

    // An enum is a natural way to store current state since it can only
    // be in one of a selection of states. 
    // you still have to control any logic that needs to fire when transitioning states
    // manually in functions however. Generally good practice for the logic 
    // to be air tight in the state machine, then allow the animations to follow that. 
    [Header("Enum of current State in the State Machine")]
    public StageOfAbility stageOfAbility;

    [Header("I'm going to use a Rigidbody for collisions (for now)")]
    public Rigidbody2D abilityRigidbody; // for enabling trigger using simulated bool in RB. 

    // --- ART STUFF ---

    // [ ] Upgrade to Animation System 

    public enum ArtStyle { Sprites, Animation }
    [Header("Swaps Between using Sprites and Animation System")]
    public ArtStyle currentArtStyle = ArtStyle.Sprites;

    [Header(" --- 2D Simple Sprites --- ")]
    // The reference to the renderer. 
    public SpriteRenderer spriteRenderer;
    // Sprite States 
    public Sprite readySprite;
    public Sprite windupSprite;
    public Sprite firingSprite;
    public Sprite winddownSprite;
    public Sprite cooldownSprite;

    [Header(" --- Animator ---  ")]
    public Animator anim;
    private string anim_windupString = "Lazer_Windup";
    private string anim_firingString = "Lazer_Firing";
    private string anim_winddownString = "Lazer_Winddown";


    // ------------------------------------------------------------------
    public bool DEBUG_MODE = false;

    // ------------------------------------------------------------------
    // --- Functions --- 
    // ------------------------------------------------------------------

    // ========================== Initialization ===========================
    protected void Start() {
        if(currentArtStyle == ArtStyle.Sprites) {

        }
    }

    // ========================== Public Facing ===========================
    // Public facing method that TRIES to activate the ability (only succeeds if it is in ready state)
    // You may want to have additional initializiation or conditions before the private start windup function runs. 
    // For example, you could check if the player has sufficient mana etc 
    public void ActivateAbility()
    {
        // Check cooldown
        // if ready
        // Start execution here. 
        if(stageOfAbility == StageOfAbility.ready) {
            StartWindup();
            if(DEBUG_MODE) 
                Debug.Log("Calling Starting Wind Up State From Activate Ability");
        }
    }

    // ========================== State Activations ===========================
    // Entering a new state functions, called but Update when first transitioning to a given state. 

    // Enter a ready state
    private void StartReady() {

        stageOfAbility = StageOfAbility.ready;

        // start ready state
        if (currentArtStyle == ArtStyle.Sprites) {
            spriteRenderer.sprite = readySprite;
        } else {
            // Should be off already but just demonstrating
            anim.SetBool(anim_windupString, false);
            anim.SetBool(anim_firingString, false);
            anim.SetBool(anim_winddownString, false);
        }
    }

    // Starting the windup, begin displaying effects, start audio, but physics is not activated yet. 
    private void StartWindup() {

        // Update to current state
        stageOfAbility = StageOfAbility.windup;

        // start animation
        if(currentArtStyle == ArtStyle.Sprites) {
            spriteRenderer.sprite = windupSprite;
        } else {
            anim.SetBool(anim_windupString, true);
        }
        

        // play sound effect
        // [ ] TO DO
        // start effects

        // Start firing timer
        currentTimer = 0;

        if (DEBUG_MODE)
            Debug.Log("Calling StartWindup State Completed");
    }

    // The ability actually activates. check the physics overlap
    // You could also do a raycast or sphere/circle (etc) cast here instead. 
    // This is a simple way for visualizing in a prototype though. 
    private void StartFiring() {

        // Update to current state
        stageOfAbility = StageOfAbility.firing;

        // next stage of animation
        if (currentArtStyle == ArtStyle.Sprites) {
            spriteRenderer.sprite = firingSprite;
        } else {
            anim.SetBool(anim_windupString, false);
            anim.SetBool(anim_firingString, true);
        }

        // Activate physics check
        abilityRigidbody.simulated = true;

        currentTimer = 0;

        if (DEBUG_MODE)
            Debug.Log("Calling StartFiring State Completed");
    }

    // Disable the physics and play the last frame of animation. 
    private void StartWinddown() {

        // Update to current state
        stageOfAbility = StageOfAbility.winddown;

        // last stage of animation
        if (currentArtStyle == ArtStyle.Sprites) {
            spriteRenderer.sprite = winddownSprite;
        } else {
            anim.SetBool(anim_firingString, false);
            anim.SetBool(anim_winddownString, true);
        }

        // disable physics check perhaps
        abilityRigidbody.simulated = false;

        // maybe immobilized or be unable to activate other abilities 
        // start animation
        

        currentTimer = 0;

        if (DEBUG_MODE)
            Debug.Log("Calling StartWinddown State Completed");
    }

    // Enter cooldown state. May shut off effects and trigger UI's
    private void StartCooldown() {
        // Update to current state
        stageOfAbility = StageOfAbility.cooldown;

        // Disable art or set cooldown art (or UI)
        if (currentArtStyle == ArtStyle.Sprites) {
            spriteRenderer.sprite = cooldownSprite;
        } else {
            anim.SetBool(anim_winddownString, false);
        }

        currentTimer = 0;
    }

    // Could be done with a simple timer setup, coroutines or an Animation

    // ========================== Update Loop ===========================

    // Update is called once per frame
    // These states are being controlled with a Timer. 
    // Could replaced with coroutines in the future (esp in Unreal Navmesh)
    // But for now this is simple. 
    void Update()
    {
        // Get Input
        currentTimer += Time.deltaTime;

        // A little awkward to put this here but its simple. 
        // Often the input would be coming from a different class however
        // and you want the AI and Player to have a similar ActivateAbility function available. 
        if (Input.GetButtonDown("Jump")) {
            ActivateAbility();
        }


        // These all just check the current state against the current state timer
        // It also transitions to the next state when the timer is up and resets it. 
        if (stageOfAbility == StageOfAbility.ready) {



            // Waiting for input. Just wait in Ready until Activated



        } else if (stageOfAbility == StageOfAbility.windup) {
            // Check timer and advance to Firing state if the timer is done
            if (currentTimer >= activatedAbility_WindupTimer) {
                // Next stage
                StartFiring();
            }
        } else if (stageOfAbility == StageOfAbility.firing) {
            // Check timer and advance to Winddown state if the timer is done
            if (currentTimer >= activatedAbility_FiringTimer) {
                // Next stage
                //  stageOfAbility = Stage
                StartWinddown();
            }
        } else if (stageOfAbility == StageOfAbility.winddown) {
            // Check timer and advance to Cooldown state if the timer is done
            if (currentTimer >= activatedAbility_WinddownTimer) {
                StartCooldown();
            }
        // Cooldown
        } else if (stageOfAbility == StageOfAbility.cooldown) {
            // Check timer and advance to Ready state if the timer is done
            if (currentTimer >= activatedAbility_CooldownTimer) {
                StartReady();
            }
        }

        // Right now cooldown is seperate but we could probably use it as a state in the state machine. 
    }


    //========================== Extra ===========================
    // Could be useful to export the ratio of current to total time to a 0..1 value range. 
    public float GetTimerCompletionRatio() {
        float returnValue = 0;
        if (stageOfAbility == StageOfAbility.cooldown) {
            return currentTimer / activatedAbility_CooldownTimer;
        } else {
            Debug.LogWarning("Not Yet Completed. This will probably crash your systems until you fix it. ");
            return -1f;
        }

        return returnValue;
    }

}
