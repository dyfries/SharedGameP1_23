using System.Collections.Generic;
using UnityEngine;

public enum FreezeMode
{
    Radius,
    Select
}
public class Ability_SlowDown : Ability_Simple
{
    [Header("Slow Down Ability Settings: ")]

    [Range(1f, 10f)]
    [SerializeField] private float radius = 5; //radius of player 
    private float previousRadius; //keep track if radius size changes
    [Range(0.1f, 1f)]
    [SerializeField] private float slowDownRate = 0.5f; // rate to add to drag to slow down object
    [SerializeField] private FreezeMode mode = FreezeMode.Radius;

    [Header("References: ")]
    private GameObject player;
    private SoundManager soundManager;
    public GameObject radiusArt;
    public GameObject freezeBlock; //freeze block object
    private Camera mainCamera;
    private GameObject radiusDrawing;

    [Header("Lists of selects objects and components: ")]
    private List<GameObject> frozenBlocks = new List<GameObject>();  //list of frozen blocks
    private List<Animator> blockAnimators = new List<Animator>(); //list of animators on blocks
    private List<GameObject> selectedObjects = new List<GameObject>();  // a list of the objects within radius

    private float initialDrag;  // A variable to kepe track of the objects initial drag
    private Transform playerTransform;
    private int layerMask;
    protected void Start()
    {
        //assign variables
        soundManager = gameObject.GetComponentInChildren<SoundManager>();
        player = gameObject.transform.parent.gameObject;
        mainCamera= Camera.main;
        layerMask = LayerMask.GetMask("NPC");
        previousRadius = radius;

        if (mode == FreezeMode.Radius) //if mode is radius draw the radius on start
        {
            DrawRadius();
        }
    }

    protected override void Update()
    {


        base.Update();

        if (mode == FreezeMode.Select)
        {
            SelectObject();
            if (radiusDrawing != null)
            {
                Destroy(radiusDrawing); //destroy the radius
            }
        }
        for(int i = 0; i< selectedObjects.Count; i++)
        {
            Debug.Log(selectedObjects[i]);
        }

        if(previousRadius != radius && radiusDrawing != null)
        {
            Destroy(radiusDrawing);
            DrawRadius();
            previousRadius = radius;
        }
    }

    // Update is called once per frame
    protected override void StartReady()
    {
        base.StartReady();

        if (mode == FreezeMode.Radius)
        {
            DrawRadius();
        }
    }

    protected override void StartWindup()
    {
        base.StartWindup();
        if (mode == FreezeMode.Radius)
        {
            CheckRadius();
        }
    }
    protected override void StartFiring()
    {
        if (radiusDrawing != null)
        {
            Destroy(radiusDrawing); //destroy the radius
        }

        base.StartFiring();
        SlowDown(); //call slow down

        SpriteRenderer objectsSR;
        Transform objectsTransform;

        for (int i = 0; i < selectedObjects.Count; i++)
        {
            objectsSR = selectedObjects[i].GetComponent<SpriteRenderer>();
            objectsTransform = selectedObjects[i].transform;
            if (objectsSR != null) 
            {
                objectsSR.color = Color.white; //change to white in case it has been selected in select mode
                soundManager.PlayFreezeSound();
                soundManager.PlayWaterDripSound();
                GameObject block = Instantiate(freezeBlock, objectsTransform.position, objectsTransform.rotation, objectsTransform); //puts the object in a freeze block
                frozenBlocks.Add(block);

                if(block.GetComponent<Animator>() != null)
                {
                    blockAnimators.Add(block.GetComponent<Animator>()); //add the blocks animator to the list of animators
                }
            }
        }

        StartMelting(); //start melting animation
    }

    protected override void StartWinddown()
    {
        base.StartWinddown();
        StopSlowDown(); //reverse slow down

        for (int i = 0; i < frozenBlocks.Count; i++)
        {
            if (blockAnimators[i] != null)
            {
                blockAnimators[i].ResetTrigger("start melting");
            }
            Destroy(frozenBlocks[i].gameObject);
        }
        soundManager.waterDripSound.Stop();
        selectedObjects.Clear();
    }


    private void CheckRadius()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(player.transform.position, radius, Vector2.zero, 0,  layerMask); //get array of objects hit by circlcast that are in NPC layer

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject !=  null) {
                selectedObjects.Add(hits[i].collider.gameObject);
            }

        }
    }
    private void SlowDown()
    {
        for (int i = 0; i < selectedObjects.Count; i++)
        {
            initialDrag = selectedObjects[i].gameObject.GetComponent<Rigidbody2D>().drag;
            selectedObjects[i].gameObject.GetComponent<Rigidbody2D>().drag = initialDrag + slowDownRate;
        }
    }

    private void StopSlowDown()
    {
        for (int i = 0; i < selectedObjects.Count; i++) // go through every object in radius
        {
            Rigidbody2D rb = selectedObjects[i].gameObject.GetComponent<Rigidbody2D>();
            BaseNPC npcScript = selectedObjects[i].gameObject.GetComponent<BaseNPC>();
            rb.drag = -slowDownRate; //reverse slow down rate
            if (rb!= null && npcScript != null) //check if game object is an NPC
            {
                float forceAmount = npcScript.startMoveForceY;
                rb.AddForce(new Vector2(0, forceAmount), ForceMode2D.Impulse); //so speed continues, using impulse right now
            }
        }
    }

    private void StartMelting()
    {
        for (int i = 0; i < blockAnimators.Count; i++)
        {
            if (blockAnimators[i] != null)
            {
                print("start ");
                blockAnimators[i].SetTrigger("start melting");
            }
        }
    }

    private void DrawRadius()
    {
        radiusDrawing = Instantiate(radiusArt, player.transform.position, player.transform.rotation, player.transform);
        radiusDrawing.transform.localScale = new Vector3(radius / 10, radius / 10, radius / 10);
    }

    private void SelectObject()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        mousePos = mainCamera.ScreenToWorldPoint(mousePos);
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0, layerMask);


        if (Input.GetMouseButtonDown(0)) //left mouse button down
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.GetComponent<SpriteRenderer>() != null)
                {
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                }
                selectedObjects.Add(hit.collider.gameObject);
            }
        }
    }
}
