using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

// Talking about force mode while creating our first NPC (using a Rigidbody2D). 
// Also timers, most basic state machine, and ForceModes
public class BaseNPC : MonoBehaviour
{

    // Move back and forth. Using physics for now. 
    public Rigidbody2D rigid;

    public float moveForceX = 1f; // An impulse, so not normalized over frame rate - use for one shots
    public float startMoveForceY = -1f; // Add force each frame, 

    // Should this NPC move back and forth?
    public bool moveSideToSide = false;
    private bool isMovingLeft = false; // if true we are moving right. 
    public float directionTimer = 1f; // how long to move in each direction before switching. 
    private float currentDirectionTimer = 0f; // countdown timer for movement. 

    // Sprites: 
    public SpriteRenderer shipBaseSprite;
    public Sprite baseMove;
    public Sprite moveLeft;
    public Sprite moveRight;

    public void Start() {
        rigid = GetComponent<Rigidbody2D>();
        shipBaseSprite = GetComponent<SpriteRenderer>();

        // On spawn, give them a push in Y. For now, they won't be able to tolerate any drag
        rigid.AddForce(new Vector2(0, startMoveForceY), ForceMode2D.Impulse);

        // We need to start off halfway through the force impulse otherwise the return force will simply cancel out the original move and it wont move correctly. 
        currentDirectionTimer = directionTimer / 2f;
    }

    private void Update() {
        // Do the state timer in here
        if (moveSideToSide) {
            // update the timer
            currentDirectionTimer = currentDirectionTimer + Time.deltaTime; // Or currentDirectionTimer += Time.deltaTime;
        
            if(currentDirectionTimer >= directionTimer) {
                // flip direction
                isMovingLeft = !isMovingLeft;
                // reset timer
                currentDirectionTimer = 0;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveSideToSide) {
            if (isMovingLeft) {
                rigid.AddForce(new Vector2(moveForceX * -1f, 0), ForceMode2D.Force); // This force mode is normalized over time. 
            } else {
                rigid.AddForce(new Vector2(moveForceX, 0), ForceMode2D.Force); // This force mode is normalized over time.
            }
        }
        

        // Art updates 
        if( rigid.velocity.x < -.3) {
            // move left sprite
            shipBaseSprite.sprite = moveLeft;
        }else if(rigid.velocity.x > .3) {
            // move right
            shipBaseSprite.sprite = moveRight;
        } else {
            shipBaseSprite.sprite = baseMove;
        }
    }
}
