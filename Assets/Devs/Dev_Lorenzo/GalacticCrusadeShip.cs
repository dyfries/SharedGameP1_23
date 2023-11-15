using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalacticCrusadeShip : MonoBehaviour
{
    private void Update()
    {
        transform.Translate(Vector2.up * Time.deltaTime * 5f);
    }
}
