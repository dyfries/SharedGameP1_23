using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastingDemo : MonoBehaviour
{

    public LayerMask layerMask; // Needs Bitmask operations
    // Start is called before the first frame update
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

    }
}
