using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    public float angle = 30f;

    // Update is called once per frame
    void Update()
    {
        Vector3 ray = Vector3.up;
        ray = Quaternion.AngleAxis(angle, Vector3.forward) * ray;
        // OR around world axis
        Debug.Log(ray);
        // vector = Q.Euler(0,40,0) * vector;
        Debug.DrawRay(Vector3.zero, ray, Color.blue);
    }
}
