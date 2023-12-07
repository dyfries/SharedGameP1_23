using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KochCurve : MonoBehaviour
{
    public int order = 4; // (depth of recursion). 
    public float length = 1f;
    public float rotationAngle = 60f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void DrawCurve(int currentOrder, Vector3 start, Vector3 end, Color curveColor) {
        Vector3 segment = end - start;
        segment = segment / 3f;
        Vector3 midPoint1 = start + segment;
        Vector3 midPoint2 = start + segment*2;

        Vector3 rotatedOffset = Quaternion.AngleAxis(rotationAngle, Vector3.forward) * segment;

        Vector3 pointOfCurve = midPoint1 + rotatedOffset;

        Debug.DrawLine(start, midPoint1, curveColor);
        Debug.DrawLine(midPoint1, pointOfCurve, curveColor);
        Debug.DrawLine(pointOfCurve, midPoint2, curveColor);
        Debug.DrawLine(midPoint2, end, curveColor);

    }

    // Update is called once per frame
    void Update()
    {
        DrawCurve(order, transform.position, transform.position + Vector3.right * length, Color.white);
    }
}
