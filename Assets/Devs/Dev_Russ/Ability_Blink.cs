using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Blink : Ability_Simple
{
    [Header("Now in Ability Blink Subclass")]
    public PlayerController pc;
    public Rigidbody2D rb;
    private Transform selfTransform;
    private Vector2 cachedSpeed;
    private Vector3 blinkDistance;
    private float baseDistance = 5f;

    //Either multiply with a base float to get a blink linked with current speed
    //Or use a base distance
    [Header("Multiplier for distance to be teleported")]
    [SerializeField] private float blinkMultiplier = 1.5f;
    [SerializeField] private bool staticDistance = false;
    protected void Start()
    {
        activatedAbility_WindupTimer = 0f;

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogWarning("No Rigidbody2D found");
            enabled = false;
        }

        pc = GetComponent<PlayerController>();
        if (pc == null)
        {
            Debug.LogWarning("No Rigidbody2D found");
            enabled = false;
        }
        else
        {

        }
        selfTransform = transform;
    }

    protected override void StartFiring()
    {

        base.StartFiring();

        if (!staticDistance)
        {
            cachedSpeed = rb.velocity;
            Debug.Log(rb.velocity);
            blinkDistance = cachedSpeed / blinkMultiplier;
            Debug.Log(blinkDistance);
        }
        else
        {
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
        }

        //Add the blink distance to the player's current position
        selfTransform.position += blinkDistance;
    }

    protected override void StartWinddown()
    {
        base.StartWinddown();
        blinkDistance = new Vector2(0, 0);
    }
}
