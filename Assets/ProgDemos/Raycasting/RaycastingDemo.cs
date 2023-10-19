using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastingDemo : MonoBehaviour
{

    public LayerMask layerMask; // Needs Bitmask operations
    // Start is called before the first frame update

    public ContactFilter2D contactFilter;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Simplest
        RaycastHit2D rayHit1 = Physics2D.Raycast(transform.position, transform.up * 10f);
        if(rayHit1.collider != null) {
          
            Debug.DrawRay(transform.position, transform.up * 10f, Color.blue);
        } else {
            Debug.DrawRay(transform.position, transform.up * 10f, Color.magenta);

        }

        /* using 3d by accident
        if( Physics.Raycast(transform.position, transform.up, 10f, layerMask.value)) {
            Debug.DrawRay(transform.position + transform.right * .1f, transform.up * 10f, Color.green);
        } else {
            Debug.DrawRay(transform.position + transform.right*.1f, transform.up * 10f, Color.red);
        }
        */
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 10f, layerMask.value);

        if (hit.collider != null) { 
            Debug.DrawRay(transform.position + transform.right * .1f, transform.up * 10f, Color.green);
        } else {
            Debug.DrawRay(transform.position + transform.right * .1f, transform.up * 10f, Color.red);
        }



        // raycast many objects using a contact filter. 
        RaycastHit2D[] arrayHits = new RaycastHit2D[10];
        int numHits = Physics2D.Raycast(transform.position, transform.up, contactFilter, arrayHits, 10f);

        Debug.Log("Objects returned: " + numHits);

        for(int i = 0; i < numHits; i++) {
            Debug.Log("HIT: " + arrayHits[i].collider.gameObject.name);
        }

    }
}
