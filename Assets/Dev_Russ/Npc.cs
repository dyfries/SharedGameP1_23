using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{

    private int currentHealth;

    /*Pull from the wavesettings team
     */
    private int speed;
    private float movementTime;


    public enum listOfPatterns
    { 
        topToBottom,
        leftToRight,
        rightToLeft,
    }
    private listOfPatterns pattern;

    // Start is called before the first frame update
    void Start()
    {
        /*
         * speed = waveSettings.GetSpeed();
         * movementTime = waveSettings.GetMovementTime();
         * listOfPatterns
         */

    }

    // Update is called once per frame
    void Update()
    {
        //Choose the movement pattern dependant on the chosen pattern
        if(pattern == listOfPatterns.topToBottom)
        {
            TopToBottom();
        }else if(pattern == listOfPatterns.leftToRight)
        {
            LeftToRight();
        }else if(pattern == listOfPatterns.rightToLeft)
        {
            RightToLeft();
        }
    }

    private void Move(Vector2 velocity, float movementTime)
    {
        /*Moves the NPC a certain direction for a set time
         * 
         * Will be used multiple times dependant on pattern
         * 
         * Get wave settings team to create variables for 
         * speed and time, will pull those variables in order to
         * adjust the NPC's movement
        */


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

    public void SetPattern(listOfPatterns pattern)
    {
        //Method that sets the pattern to be used
        this.pattern = pattern;
    }

    //Methods that dictate movement dependant on pattern
    private void TopToBottom()
    {

    }

    private void LeftToRight()
    {

    }

    private void RightToLeft()
    {

    }
}
