using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ability_Blink : Ability_Simple
{
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
    [SerializeField] private SpriteRenderer sr;
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
        //We want almost instant teleportation
        teleportWindup = transform.GetChild(1).gameObject.GetComponent<AudioSource>();
        teleportInstantate = transform.GetChild(2).gameObject.GetComponent<AudioSource>();
        rb = GetComponentInParent<Rigidbody2D>();
        sr = blinkBubble.GetComponent<SpriteRenderer>();
        if (teleportWindup == null)
        {
            Debug.LogWarning("No AudioSource detected for first effect");
        }
        if (teleportInstantate == null)
        {
            Debug.LogWarning("No AudioSource detected for second effect");
        }
        if (rb == null)
        {
            Debug.LogWarning("No Rigidbody2D found");
            enabled = false;
        }
        if (sr == null)
        {
            Debug.LogWarning("No SpriteRenderer found");
            enabled = false;
        }
        if (anim == null)
        {
            anim = blinkBubble.GetComponent<Animator>();
            Debug.LogWarning("Animator cannot be found");
        }
        blinkBubble.SetActive(false);
    }

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
        teleportInstantate.Play();
        Instantiate(teleportEffects, blinkDistance, new Quaternion(0, 0, 0, 0));
    }

    protected override void StartWinddown()
    {
        base.StartWinddown();

        //Return blinkDistance to 0, to prevent continuous growth
        blinkDistance = new Vector2(0, 0);
        blinkBubble.SetActive(false);
    }

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