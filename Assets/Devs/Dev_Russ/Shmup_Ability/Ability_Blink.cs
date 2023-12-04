using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ability_Blink : Ability_Simple
{
    // Private fields for various components and parameters
    [Header("Ability Blink Subclass")]
    private Rigidbody2D rb;
    private Vector2 cachedSpeed;
    private Vector2 blinkDistance;
    private AudioSource teleportWindup;
    private AudioSource teleportInstantate;
    [SerializeField] private float baseDistance = 5f;

    [Header("Animations")]
    public Animator anim;
    public float colourSpeed;
    private SpriteRenderer sr;
    [SerializeField] private GameObject blinkBubble;


    //Either multiply with a base float to get a blink linked with current speed
    //Or use a base distance
    [Header("Divisor for distance to be teleported")]
    [SerializeField] private float blinkDivisor = 1.5f;
    [SerializeField] private bool staticDistance = false;

    [Header("Particle System")]
    public GameObject teleportEffects;

    protected void Start()
    {
        teleportWindup = transform.GetChild(1).gameObject.GetComponent<AudioSource>();
        teleportInstantate = transform.GetChild(2).gameObject.GetComponent<AudioSource>();
        rb = GetComponentInParent<Rigidbody2D>();
        sr = blinkBubble.GetComponent<SpriteRenderer>();

        // Null checks
        if (teleportWindup == null || teleportInstantate == null || rb == null || sr == null)
        {
            Debug.LogError("One or more components are missing. Check the setup.");
            enabled = false;
        }

        anim = blinkBubble.GetComponent<Animator>();
        if (anim == null)
        {
            anim = blinkBubble.AddComponent<Animator>();
            Debug.LogWarning("Animator cannot be found, adding a new one.");
        }

        blinkBubble.SetActive(false);
    }

    // Method to start the windup phase of the ability
    protected override void StartWindup()
    {
        base.StartWindup();
        StartCoroutine(ChangeBubbleColour());
        blinkBubble.SetActive(true);
        anim.SetBool("Windup", true);
        teleportWindup.Play();
    }
    protected override void StartFiring()
    {
        anim.SetBool("Windup", false);
        base.StartFiring();
        sr.color = new Color(1, 1, 1, 0.5f);

        // Calculate the blink distance based on velocity or set distance
        if (!staticDistance)
        {
            cachedSpeed = rb.velocity;
            blinkDistance = cachedSpeed / blinkDivisor;
        }
        else
        {
            // Set distance based on velocity direction using Mathf.Sign
            blinkDistance.x += Mathf.Sign(rb.velocity.x) * baseDistance;
            blinkDistance.y += Mathf.Sign(rb.velocity.y) * baseDistance;
        }

        // Move the player to the blink distance
        rb.MovePosition(blinkDistance);
        // Play sound Effect
        teleportInstantate.Play();
        Instantiate(teleportEffects, blinkDistance, Quaternion.identity);
    }

    // Method to start the winddown phase of the ability
    protected override void StartWinddown()
    {
        base.StartWinddown();

        // Reset blinkDistance to prevent continuous growth
        blinkDistance = new Vector2(0, 0);
        blinkBubble.SetActive(false);
    }

    // Coroutine to gradually change the color of the blink bubble during windup
    private IEnumerator ChangeBubbleColour()
    {
        float tick = 0f;
        float timer = 0f;
        while (timer < activatedAbility_WindupTimer)
        {
            tick += Time.deltaTime * colourSpeed;
            timer += Time.deltaTime;
            sr.color = Color.Lerp(new Color(1, 1, 1, 0f), Color.red, tick);
            Debug.Log("Running coroutine");
            yield return null;
        }
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
 *  [0] Incorporate blink animations
 *  *SCRAPED*[] Create enum state that provides level of ability //subject to change for other ways
 *  *SCRAPED*[] Create variables for level 1
 *  [0] Barrier around player on exit
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