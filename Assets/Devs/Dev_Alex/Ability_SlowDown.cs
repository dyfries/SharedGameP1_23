using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Ability_SlowDown : Ability_Simple
{
    // Start is called before the first frame update
    private int radius = 5;
    private float slowDownRate = 0.5f;
    private List<GameObject> objectsInRadius = new List<GameObject>(); 
    private float initialDrag; 
    protected void Start()
    {
        gameObject.GetComponent<CircleCollider2D>().radius = radius;
        gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
    }

    // Update is called once per frame
    protected override void StartFiring()
    {
        base.StartFiring();
        for(int i =0; i< objectsInRadius.Count; i++)
        {
            initialDrag = objectsInRadius[i].gameObject.GetComponent<Rigidbody2D>().drag;
            objectsInRadius[i].gameObject.GetComponent<Rigidbody2D>().drag = initialDrag+ slowDownRate;
        }
    }

    protected override void StartWinddown()
    {
        base.StartWinddown();
        for (int i = 0; i < objectsInRadius.Count; i++)
        {
            initialDrag = objectsInRadius[i].gameObject.GetComponent<Rigidbody2D>().drag;
            objectsInRadius[i].gameObject.GetComponent<Rigidbody2D>().drag = -slowDownRate;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null) {
            objectsInRadius.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (objectsInRadius.Contains(collision.gameObject)){
            objectsInRadius.Remove(collision.gameObject);
        }
    }


}
