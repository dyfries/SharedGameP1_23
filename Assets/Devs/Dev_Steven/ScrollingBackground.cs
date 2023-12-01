using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float scrollingSpeed = 5.0f;
    public float backgroundSize = 25f;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = transform.position + Vector3.down * (scrollingSpeed) * Time.deltaTime;

        if (transform.position.y < -backgroundSize)
        {
            transform.position = startPosition;
        }
    }
}