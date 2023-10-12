using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Destroy Self in N seconds
public class SimpleSelfDestruction : MonoBehaviour
{

    public float deathTimer = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, deathTimer);
    }

}
