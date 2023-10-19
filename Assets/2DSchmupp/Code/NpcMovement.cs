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
    [SerializeField] private int speed = 1;
    [Range(1f,3f)]
    [SerializeField] private float flipTime = 3f;

    private int flip = 1;
    private float timer = 0;
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

    public void SetSpeed(int speed)
    {
        this.speed = speed;
    }

    public int GetSpeed()
    {
        return this.speed;
    }

    public void SetTime(float time)
    {
        flipTime = time;
    }

    public float GetTime()
    {
        return flipTime;
    }
    private void TakeDamage(int damage)
    {
        //Method that deducts hp from NPC
        currentHealth -= damage;
    }

    public void SetPattern(listOfPatterns pattern)
    {
        //Method that sets the pattern to be used
        this.pattern = pattern;
    }

    //Methods that dictate movement dependant on pattern
    private void TopToBottom()
    {
        Move(new Vector2(0, -1));
    }

    private void LeftToRight()
    {
        Move(new Vector2(1, 0));
    }

    private void RightToLeft()
    {
        Move(new Vector2(-1, 0));
    }

    private void ZigZag()
    {
        timer += Time.deltaTime;
        if(timer >= flipTime) //Flip the left/right direction
        {
            flip = -flip;
            rigid.velocity = new Vector2(0, 0);
            timer = 0;
        }
        Move(new Vector2(flip, -1));

    }

    private void VShape()
    {
        timer += Time.deltaTime;
        if(timer >= flipTime && goUp == false)
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
