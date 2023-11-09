using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Blink : Ability_Simple
{
    [Header("Ability Blink Subclass")]
    public Rigidbody2D rb;
    private Vector2 cachedSpeed;
    private Vector2 blinkDistance;
    private float baseDistance = 5f;

    //Either multiply with a base float to get a blink linked with current speed
    //Or use a base distance
    [Header("Multiplier for distance to be teleported")]
    [SerializeField] private float blinkMultiplier = 1.5f;
    [SerializeField] private bool staticDistance = false;
    protected void Start()
    {
        //We want instant teleportation
        activatedAbility_WindupTimer = 0f;

        rb = GetComponentInParent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogWarning("No Rigidbody2D found");
            enabled = false;
        }
    }

    protected override void StartFiring()
    {

        base.StartFiring();

        if (!staticDistance)
        {//Distance based on velocity
            cachedSpeed = rb.velocity;
            Debug.Log(rb.velocity);
            blinkDistance = cachedSpeed / blinkMultiplier;
            Debug.Log(blinkDistance);
        }
        else
        {//Set distance
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