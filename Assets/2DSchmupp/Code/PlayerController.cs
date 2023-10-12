using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] 
public class PlayerController : MonoBehaviour
{
    // Basic Movement
    [Header("Basic Movement")]
    // Using a V2 so we can adjust horizontal and vertical move speed seperately. 
    // We will be using physics here, so this will be a force. 
    public Vector2 moveForce = new Vector2(1f, 1f);


    [Header("Cached Vars - Don't Touch")]
    private Vector2 directionalInput;
    private Rigidbody2D physicsBody; 

    // Start is called before the first frame update
    void Start()
    {

        physicsBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        directionalInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void FixedUpdate() {

        physicsBody.AddForce(moveForce * directionalInput); // don't need the delta time in fixed update. 
    }
}
