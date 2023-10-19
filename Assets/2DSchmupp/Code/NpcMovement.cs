using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NpcMovement : MonoBehaviour
{

    private int currentHealth;

    [Header("Flight Settings")]
    //Flight Settings to adjust how fast NPC moves, and when to switch directions
    [Range(1, 5)]
    [SerializeField] private int speed = 1; //How fast the NPC will move
    [Range(1f,3f)]
    [SerializeField] private float flipTime = 3f; //How long to move a certain direction before switching

    private int flip = 1; //Controls whether or not the Vector2 is moving in a positive or negative direction on axis
    private float currentDirectionTimer = 0; //Countdown time for movement
    private Transform position;
    private Rigidbody2D rigid;
    private bool goUp = false;

    [Header("Flight Pattern")]
    //List of patterns to select from
    [SerializeField] public listOfPatterns pattern;


    // Start is called before the first frame update
    void Start()
    {

        position = this.transform;

        rigid = this.GetComponent<Rigidbody2D>();

 
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        //Choose the movement pattern dependant on the chosen pattern
        if (pattern == listOfPatterns.topToBottom)
        {
            TopToBottom();
        }
        else if (pattern == listOfPatterns.leftToRight)
        {
            LeftToRight();
        }
        else if (pattern == listOfPatterns.rightToLeft)
        {
            RightToLeft();
        }
        else if (pattern == listOfPatterns.topLeftToBottomRight)
        {
            TopToBottom();
            LeftToRight();
        }
        else if (pattern == listOfPatterns.zigZag)
        {
            ZigZag();
        }
        else if (pattern == listOfPatterns.vShape)
        {
            VShape();
        }
    }

    private void Move(Vector2 velocity)
    {
        /*Moves the NPC a certain direction for a set time
         * 
         * Will be used multiple times dependant on pattern
        */

        rigid.AddForce(velocity * speed);

    }

    public int GetHealth()
    {
        ///Get Method that returns NPC's Health
        return currentHealth;
    }

    public void SetHealth(int health)
    {
        //Set Method that sets the NPC's Health
        currentHealth = health;
    }
    private void TakeDamage(int damage)
    {
        //Method that deducts hp from NPC
        currentHealth -= damage;
    }


    public void SetSpeed(int speed)
    {
        //Set Methods that sets the NPC's Speed
        this.speed = speed;
    }

    public int GetSpeed()
    {
        //Get Method that gets the NPC's speed
        return this.speed;
    }

    public void SetTime(float time)
    {
        //Set Method that sets the NPC's flip timer
        flipTime = time;
    }

    public float GetTime()
    {
        //Get Method that Gets the NPC's flip timer
        return flipTime;
    }
    public void SetPattern(listOfPatterns pattern)
    {
        //Method that sets the pattern to be used
        this.pattern = pattern;
    }

    //Methods that dictate movement dependant on pattern
    private void TopToBottom()
    {
        //Stub to be revamped in future
        Move(new Vector2(0, -1));
    }

    private void LeftToRight()
    {
        //Stub to be revamped in future
        Move(new Vector2(1, 0));
    }

    private void RightToLeft()
    {
        //Stub to be revamped in future
        Move(new Vector2(-1, 0));
    }

    private void ZigZag()
    {
        //Stub to be revamped in future
        currentDirectionTimer += Time.deltaTime;
        if(currentDirectionTimer >= flipTime) //Flip the left/right direction
        {
            flip = -flip;
            rigid.velocity = new Vector2(0, 0);
            currentDirectionTimer = 0;
        }
        Move(new Vector2(flip, -1));

    }

    private void VShape()
    {
        //Stub to be revamped in future
        currentDirectionTimer += Time.deltaTime;
        if(currentDirectionTimer >= flipTime && goUp == false)
        {
            rigid.velocity = new Vector2(0, 0);
            goUp = true;
        }

        if (goUp == false) //First half of movement v
        {
            Move(new Vector2(1, -1));
        }
        else //Second half of movement v
        {
            Move(new Vector2(1, 1));
        }
    }
    
}

public enum listOfPatterns
{
    topToBottom,
    leftToRight,
    rightToLeft,
    topLeftToBottomRight,
    zigZag,
    vShape
}
