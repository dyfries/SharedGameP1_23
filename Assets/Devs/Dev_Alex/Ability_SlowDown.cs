using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Ability_SlowDown : Ability_Simple
{
    [Header("Slow Down Ability Settings")]
    [SerializeField] private int radius = 5; //radius of player 
    [SerializeField] private float slowDownRate = 0.5f; // rate to add to drag to slow down object

    private List<GameObject> objectsInRadius = new List<GameObject>();  // a list of the objects within radius
    private float initialDrag;  // A variable to kepe track of the objects initial drag
    public GameObject freezeBlock;
    private List<GameObject> frozenBlocks = new List<GameObject>(); 

    public Animator animator;
    protected void Start()
    {
        gameObject.AddComponent<CircleCollider2D>();
        gameObject.GetComponent<CircleCollider2D>().isTrigger = true; //makes the collider a trigger
        gameObject.GetComponent<CircleCollider2D>().radius = radius; //sets player's radius to the radius variable
        animator = freezeBlock.GetComponent<Animator>();
   
    }

    // Update is called once per frame
    protected override void StartWindup()
    {
        base.StartWindup(); 

        //animation
   
    }
    protected override void StartFiring()
    {
        base.StartFiring();
        SlowDown(); //call slow down

        //animation
        //going to implement a freeze blast animation and an animation that freezes objects in radius
        for (int i = 0; i < objectsInRadius.Count; i++)
        {
            if (objectsInRadius[i].GetComponent<SpriteRenderer>() != null)
            {
                GameObject block = Instantiate(freezeBlock, objectsInRadius[i].transform.position, objectsInRadius[i].transform.rotation, objectsInRadius[i].transform); //puts the object in a freeze block
                frozenBlocks.Add(block);
            }
        }
        startMelting();
    }

    protected override void StartWinddown()
    {
        base.StartWinddown();
        StopSlowDown(); //reverse slow down

        //animation
        animator.ResetTrigger("start melting");
        for (int i = 0; i < frozenBlocks.Count; i++)
        {
             Destroy(frozenBlocks[i].gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null) {
            objectsInRadius.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (objectsInRadius.Contains(collision.gameObject)){
            objectsInRadius.Remove(collision.gameObject);
        }
    }
    
    private void SlowDown()
    {
        for (int i = 0; i < objectsInRadius.Count; i++)
        {
            initialDrag = objectsInRadius[i].gameObject.GetComponent<Rigidbody2D>().drag;
            objectsInRadius[i].gameObject.GetComponent<Rigidbody2D>().drag = initialDrag + slowDownRate;
        }
    }

    private void StopSlowDown()
    {
        for (int i = 0; i < objectsInRadius.Count; i++) // go through every object in radius
        {
            objectsInRadius[i].gameObject.GetComponent<Rigidbody2D>().drag = -slowDownRate; //reverse slow down rate
            if (objectsInRadius[i].gameObject.name.Contains("NPC")) //check if game object is an NPC
            {
                Rigidbody2D rb = objectsInRadius[i].gameObject.GetComponent<Rigidbody2D>();
                float forceAmount = objectsInRadius[i].gameObject.GetComponent<BaseNPC>().startMoveForceY;
                rb.AddForce(new Vector2 (0,forceAmount), ForceMode2D.Impulse); //so speed continues, using impulse right now
            }
        }
    }

    private void startMelting()
    {
        animator.SetTrigger("start melting");
 
    }
}
