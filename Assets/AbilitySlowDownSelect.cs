using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AbilitySlowDownSelect : Ability_Simple
{
    [Header("Slow Down Ability Settings")]
    [SerializeField] private float slowDownRate = 0.5f; // rate to add to drag to slow down object

    private List<GameObject> objectsInRadius = new List<GameObject>();  // a list of the objects within radius
    private float initialDrag;  // A variable to kepe track of the objects initial drag
    public GameObject freezeBlock; //freeze block object
    private List<GameObject> frozenBlocks = new List<GameObject>();  //list of frozen blocks
    public List<Animator> blockAnimators = new List<Animator>(); //list of animators on blocks

    public GameObject player;
    private SoundManager soundManager;

    //ray casting
    public Camera mainCamera;
    protected void Start()
    {

        //assign sounds
        soundManager = gameObject.GetComponentInChildren<SoundManager>();
    }
    protected override void Update()
    {
        base.Update();

        SelectObject();
       // DrawLaser();
    }
    protected override void StartWindup()
    {
        base.StartWindup();
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
            objectsInRadius[i].GetComponent<SpriteRenderer>().color = Color.white;
            if (objectsInRadius[i].GetComponent<SpriteRenderer>() != null && objectsInRadius[i].name.Contains("NPC")) //had to check contains npc so walls dont get blocks
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
            if (blockAnimators[i] != null)
            {
                blockAnimators[i].SetTrigger("start melting");
            }

        }
    }

  private void SelectObject()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        mousePos = mainCamera.ScreenToWorldPoint(mousePos);
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (Input.GetMouseButtonDown(0)) //left mouse button down
        {
            if (hit.collider.gameObject.name.Contains("NPC") && hit.collider != null)
            {

                hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                objectsInRadius.Add(hit.collider.gameObject);
            }
        }
    }

    /*
    private void DrawLaser()
    {

        for(int i = 1; i < objectsInRadius.Count; i++)
        {
            if(objectsInRadius != null)
            {
                gameObject.AddComponent<LineRenderer>();
                gameObject.GetComponent<LineRenderer>().widthMultiplier = 0.15f;
                gameObject.GetComponent<LineRenderer>().SetPosition(0, player.transform.position);
                gameObject.GetComponent<LineRenderer>().SetPosition(i, objectsInRadius[i].transform.position);
            }
           
        }
       
    }
      */
}
