using System.Collections.Generic;
using UnityEngine;

public class Ability_SlowDown : Ability_Simple
{
    [Header("Slow Down Ability Settings")]
    [SerializeField] private int radius = 5; //radius of player 
    [SerializeField] private float slowDownRate = 0.5f; // rate to add to drag to slow down object

    private List<GameObject> objectsInRadius = new List<GameObject>();  // a list of the objects within radius
    private float initialDrag;  // A variable to kepe track of the objects initial drag
    public GameObject freezeBlock; //freeze block object
    private List<GameObject> frozenBlocks = new List<GameObject>();  //list of frozen blocks
    public List<Animator> blockAnimators = new List<Animator>(); //list of animators on blocks

    private SoundManager soundManager;
    public LayerMask NPCLayer;
    public GameObject player;

    private GameObject radiusDrawing;
    public GameObject radiusArt;
    protected void Start()
    {
        /*
        gameObject.AddComponent<CircleCollider2D>(); //add a circle collider
        gameObject.GetComponent<CircleCollider2D>().isTrigger = true; //makes the collider a trigger
        gameObject.GetComponent<CircleCollider2D>().radius = radius; //sets player's radius to the radius variable
        */
        //assign sounds
        soundManager = gameObject.GetComponentInChildren<SoundManager>();
    }

    // Update is called once per frame
    protected override void StartReady()
    {
        base.StartReady();
        print("test");
    }
    protected override void StartWindup()
    {
        base.StartWindup();
        CheckRadius();
    }
    protected override void StartFiring()
    {

        soundManager.PlayFreezeSound();
        base.StartFiring();
        SlowDown(); //call slow down

        //animation
        //going to implement a freeze blast animation and an animation that freezes objects in radius
        for (int i = 0; i < objectsInRadius.Count; i++)
        {
            if (objectsInRadius[i].GetComponent<SpriteRenderer>() != null) //had to check contains npc so walls dont get blocks
            {
                GameObject block = Instantiate(freezeBlock, objectsInRadius[i].transform.position, objectsInRadius[i].transform.rotation, objectsInRadius[i].transform); //puts the object in a freeze block
                frozenBlocks.Add(block);
                blockAnimators.Add(block.GetComponent<Animator>());

            }
        }
        
    }

    protected override void StartWinddown()
    {
        base.StartWinddown();
        StopSlowDown(); //reverse slow down
        StartMelting();
       
        for (int i = 0; i < frozenBlocks.Count; i++)
        {
            if (blockAnimators[i] != null)
            {
                blockAnimators[i].ResetTrigger("start melting");
            }
            Destroy(frozenBlocks[i].gameObject);
        }
    }


    private void CheckRadius()
    {
        
        RaycastHit2D[] hits = Physics2D.CircleCastAll(player.transform.position, radius, Vector2.zero, NPCLayer);
        for(int i=0; i<hits.Length; i++)
        {
            objectsInRadius.Add(hits[i].collider.gameObject);
        }

    }
    private void SlowDown()
    {
        Debug.Log(objectsInRadius.ToString());
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
                rb.AddForce(new Vector2(0, forceAmount), ForceMode2D.Impulse); //so speed continues, using impulse right now
            }
        }
        soundManager.waterDripSound.Stop();
    }

    private void StartMelting()
    {
        soundManager.PlayWaterDripSound();
        for (int i = 0; i < blockAnimators.Count; i++)
        {
            if (blockAnimators[i] != null){
                blockAnimators[i].SetTrigger("start melting");
            }
        
        }
    }

    private void DrawRadius()
    {
        radiusDrawing = Instantiate(radiusArt, player.transform.position, Quaternion.identity);
    }

}
