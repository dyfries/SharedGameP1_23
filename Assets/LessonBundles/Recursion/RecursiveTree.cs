using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursiveTree : MonoBehaviour
{
    [Range(1, 20)]
    public int depth = 4;
    
    [Range(0, 90)]
    public int offsetAngle = 20;
    
    [Range(.1f, 20f)]
    public float length = 1f;

    // Reduce/increase length by multiplying by this amount each time
    [Range(.1f, 2)]
    public float lengthMultiplier;

    // Start is called before the first frame update
    void Update()
    {
        DrawBranch(depth, transform.position, 0, length);
    }

    public void DrawBranch(int depth, Vector3 startPosition, int angle, float length) {

        Debug.Log("Draw Branch Called with (depth):" + depth + " (position):" + startPosition + "(angle):" + angle);
        
        if (depth > 0) {
            // Set starting branches
            Vector2 ray = Vector3.up * length;
            Vector2 ray2 = Vector3.up * length;

            // Apply rotation to vectors
            // We are rotating AROUND the forward direction (Z+)
            ray = Quaternion.AngleAxis(angle + offsetAngle, Vector3.forward) * ray;
            ray2 = Quaternion.AngleAxis(angle - offsetAngle, Vector3.forward) * ray2;

            // Draw the lines we want. 
            Debug.DrawRay(startPosition, ray);
            Debug.DrawRay(startPosition, ray2);

            // Call recursion
            DrawBranch(depth - 1, startPosition + (Vector3)ray, angle + offsetAngle, length * lengthMultiplier);
            DrawBranch(depth - 1, startPosition + (Vector3)ray2, angle - offsetAngle, length * lengthMultiplier);
        } else {
            // Done
        }
    }

}
