using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KochDrawing : MonoBehaviour
{
    public int maxOrder = 4; // (depth of recursion). 
    public float startLength = 1f;
    public float rotationAngle = 60f;

    [Range(0,1f)]
    public float upDirectionRandomProb = 1f; // add random variance to the direction of the points. .5 

    public float updateTimer = 2f; // regenerate each this many seconds. 
    private float currentUpdateTimer = 0;

    private bool ignoreSteps = false;

    void DrawCurve(int currentOrder, Vector3 start, Vector3 end, Color curveColor) {

        if( currentOrder <= maxOrder) {
            Vector3 segment = end - start;
            segment = segment / 3f;
            Vector3 midPoint1 = start + segment;
            Vector3 midPoint2 = start + segment * 2;

            // This value will get replaced but needs to live outside of the if statement. 
            Vector3 rotatedOffset = Vector3.up;

            // If this is checked, randomly flip the direction of the points from the line. 

            if(Random.value > upDirectionRandomProb) {
                rotatedOffset = Quaternion.AngleAxis(rotationAngle, Vector3.forward) * segment;
            } else {
                rotatedOffset = Quaternion.AngleAxis(-rotationAngle, Vector3.forward) * segment;
            }   

            Vector3 pointOfCurve = midPoint1 + rotatedOffset;

            if(currentOrder == maxOrder) {
                curveColor = Color.red;
            }

            // Draw the actual curve
            // update timer was added as duration since we are now only generating this once every t seconds. 
            Debug.DrawLine(start, midPoint1, curveColor, updateTimer);
            Debug.DrawLine(midPoint1, pointOfCurve, curveColor, updateTimer);
            Debug.DrawLine(pointOfCurve, midPoint2, curveColor, updateTimer);
            Debug.DrawLine(midPoint2, end, curveColor, updateTimer);

            // Recursive Case
            DrawCurve(currentOrder + 1, start, midPoint1, curveColor);
            DrawCurve(currentOrder + 1, midPoint1, pointOfCurve, curveColor);
            DrawCurve(currentOrder + 1, pointOfCurve, midPoint2, curveColor);
            DrawCurve(currentOrder + 1, midPoint2, end, curveColor);
        }
        

    }

    // Start is called before the first frame update
    void Start() {
        DrawCurve(0, transform.position, transform.position + Vector3.right * startLength, Color.white);
        currentUpdateTimer = updateTimer;
    }

    // Update is called once per frame
    void Update()
    {
        currentUpdateTimer -= Time.deltaTime;
        if(currentUpdateTimer < 0) {
            DrawCurve(0, transform.position, transform.position + Vector3.right * startLength, Color.white);
            currentUpdateTimer = updateTimer;
        }

    }
}
