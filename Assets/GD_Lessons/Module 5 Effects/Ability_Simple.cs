using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
public enum StageOfAbility {ready, windup, firing, winddown, cooldown };
public class Ability_Simple : MonoBehaviour {

    [Header("Timers")]
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


    public Rigidbody2D abilityRigidbody; // for enabling trigger using simulated bool in RB. 

    // art stuff
    public SpriteRenderer spriteRenderer;
    public Sprite readySprite;
    public Sprite windupSprite;
    public Sprite firingSprite;
    public Sprite winddownSprite;
    public Sprite cooldownSprite;


    // Start is called before the first frame update
    // Public facing method. You may want to have additional initializiation before the private start windup function runs. 
    // For example, you could check if the cooldown is activated here, if the player has sufficient mana etc 
    public void ActivateAbility()
    {
        // Check cooldown
        // if ready
        // Start execution here. 
        if(stageOfAbility == StageOfAbility.ready) {
            StartWindup();
            Debug.Log("Calling Starting Wind Up State From Activate Ability");
        }
    }

    // Entering a new state functions

    private void StartReady() {
        stageOfAbility = StageOfAbility.ready;
        spriteRenderer.sprite = readySprite;
    }

    // Starting the windup, begin displaying effects, start audio, but physics is not activated yet. 
    private void StartWindup() {
        stageOfAbility = StageOfAbility.windup;

        // start animation
        spriteRenderer.sprite = windupSprite;
        // Start firing timer

        // play sound effect
        // start effects
        currentTimer = 0;

        Debug.Log("Calling StartWindup State Completed");
    }

    // The ability actually activates. check the physics overlap
    // You could also do a raycast or sphere/circle (etc) cast here instead. 
    // This is a simple way for visualizing in a prototype though. 
    private void StartFiring() {
        stageOfAbility = StageOfAbility.firing;
        // next stage of animation
        spriteRenderer.sprite = firingSprite;
        // Activate physics check
        abilityRigidbody.simulated = true;
        Debug.Log("Calling StartFiring State Completed");

        currentTimer = 0;
    }

    // Disable the physics and play the last frame of animation. 
    private void StartWinddown() {
        stageOfAbility = StageOfAbility.winddown;
        // last stage of animation, 
        spriteRenderer.sprite = winddownSprite;
        // disable physics check perhaps
        abilityRigidbody.simulated = false;
        // maybe immobilized or be unable to activate other abilities 
        Debug.Log("Calling StartWinddown State Completed");

        currentTimer = 0;
    }

    private void StartCooldown() {
        stageOfAbility = StageOfAbility.cooldown;
        spriteRenderer.sprite = cooldownSprite;

        currentTimer = 0;
    }

    // Could be done with a simple timer setup, coroutines or an Animation


    // Update is called once per frame
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



            // Waiting for input. 



        } else if (stageOfAbility == StageOfAbility.windup) {
            if (currentTimer >= activatedAbility_WindupTimer) {
                // Next stage
                StartFiring();
            }
        } else if (stageOfAbility == StageOfAbility.firing) {
            if (currentTimer >= activatedAbility_FiringTimer) {
                // Next stage
                //  stageOfAbility = Stage
                StartWinddown();
            }
        } else if (stageOfAbility == StageOfAbility.winddown) {
            if (currentTimer >= activatedAbility_WinddownTimer) {
                StartCooldown();
            }
        // Cooldown
        } else if (stageOfAbility == StageOfAbility.cooldown) {
            if (currentTimer >= activatedAbility_CooldownTimer) {
                StartReady();
            }
        }

        // Right now cooldown is seperate but we could probably use it as a state in the state machine. 
    }

     
}
