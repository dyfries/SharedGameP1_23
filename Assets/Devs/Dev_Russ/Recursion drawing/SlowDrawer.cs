using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koch_DrawHelper : MonoBehaviour
{
    public Vector3 start;
    public Vector3 end;
    private Vector3 currPos;
    public float duration = 5f;
    private bool reach = false;

    private void Start()
    {
    }

    private void Update()
    {
        Drawer(start, end);
        
    }

    private void Drawer(Vector3 begin, Vector3 final)
    {
        Debug.DrawLine(start, currPos, Color.red);
    }
    IEnumerator DrawLineCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            currPos = Vector3.Lerp(start, end, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        reach = true;
    }
}
