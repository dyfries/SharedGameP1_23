using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Some simple utility functions for dealing with rigidbodies (2D)
[RequireComponent(typeof(Rigidbody2D))]
public class SimpleForce2D : MonoBehaviour
{
    [Header("Set the initial force on this object")]
    public bool applyInitialImpulse = false;
    public Vector2 initialImpulseForce = Vector2.zero;

    [Header("Add Force over Time")]
    public bool addForceOverTime = false;
    public Vector2 overTimeForce = Vector2.zero;

    //[Header("Radial Force")]

    [Header("Component References")]
    private Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();    

        if (applyInitialImpulse) {
            rigid.AddForce(initialImpulseForce, ForceMode2D.Impulse);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (addForceOverTime) {
            rigid.AddForce(overTimeForce, ForceMode2D.Force); // this is the default force mode if you leave the parameter out. 
        }
    }
}
