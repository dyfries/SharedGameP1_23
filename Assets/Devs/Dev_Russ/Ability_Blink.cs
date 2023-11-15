using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Blink : Ability_Simple
{
    [Header("Ability Blink Subclass")]
    public Rigidbody2D rb;
    private Vector2 cachedSpeed;
    private Vector2 blinkDistance;
    [SerializeField] private float baseDistance = 5f;

    [Header("Animations")]
    public Animator anim;
    [SerializeField] private GameObject blinkBubble;


    //Either multiply with a base float to get a blink linked with current speed
    //Or use a base distance
    [Header("Divisor for distance to be teleported")]
    [SerializeField] private float blinkDivisor = 1.5f;
    [SerializeField] private bool staticDistance = false;
    protected void Start()
    {
        //We want almost instant teleportation

        blinkBubble.SetActive(false);
        rb = GetComponentInParent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogWarning("No Rigidbody2D found");
            enabled = false;
        }

        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
            if (anim == null)
            {
                Debug.LogWarning("Animator cannot be found by Huge Lazer ability");
            }
        }

    }

    protected override void StartWindup()
    {
        base.StartWindup();
        blinkBubble.SetActive(true);
        anim.SetBool("Windup", true);
    }
    protected override void StartFiring()
    {
        anim.SetBool("Windup", false);
        base.StartFiring();
        // Will choose what kind of movement is used dependant on whether boolean is on
        if (!staticDistance)
        {// Distance based on velocity
            cachedSpeed = rb.velocity;
            Debug.Log(rb.velocity);
            blinkDistance = cachedSpeed / blinkDivisor;
            Debug.Log(blinkDistance);
        }
        else
        {// Set distance
            if (rb.velocity.x < -1)
            {
                blinkDistance.x -= baseDistance;
            }
            else if (rb.velocity.x > 1)
            {
                blinkDistance.x += baseDistance;
            }
            if (rb.velocity.y < -1)
            {
                blinkDistance.y -= baseDistance;
            }
            else if (rb.velocity.y > 1)
            {
                blinkDistance.y += baseDistance;
            }
            Debug.Log(blinkDistance);
        }

        //Add the blink distance to the player's current position
        rb.MovePosition(blinkDistance);
    }

    protected override void StartWinddown()
    {
        base.StartWinddown();

        //Return blinkDistance to 0, to prevent continuous growth
        blinkDistance = new Vector2(0, 0);
        blinkBubble.SetActive(false);
    }

}


//Task List
/*
 * [o] Base blink mechanic working (bugs are fine rn)
 * 
 * Incorporate levels of the ability (3 levels for starters)
 *      Level 1: base blink, with barrier that damages set area around player
 *      Level 2: lower the cooldown, enlarge the barrier
 *      Level 3: allow barrier to reverse projectiles velocity
 *      
 * 
 * Level 1:
 *  [0] Base blink movement
 *  [] Incorporate blink animations
 *  [] Create enum state that provides level of ability //subject to change for other ways
 *  [] Create variables for level 1
 *  [] Barrier around player on exit
 *
 * Level 2:
 *  [] Create variables for level 2
 *  [] Enlarge barrier
 *  
 * Level 3:
 *  [] Create collider detection method
 *  [] Reverse rigidbody2d velocities affected
 * 
 * 
 * 
 * 
 * 
 * 
 */