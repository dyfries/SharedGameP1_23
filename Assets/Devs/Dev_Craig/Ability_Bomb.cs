using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Bomb : Ability_Simple
{
    private CircleCollider2D bombCollider;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D spawnedCollider;

    //Acquire virtual functions.
    private void Awake()
    {
        bombCollider = GetComponent<CircleCollider2D>();
        bombCollider.enabled = false;
    }

    protected override void StartWindup()
    {
        base.StartWindup();
        //Instansiate Projectile
        GameObject spawnedBomb = Instantiate(this.gameObject, new Vector2(0f, 0f), Quaternion.identity);
        spriteRenderer = spawnedBomb.GetComponent<SpriteRenderer>();
        spawnedCollider = spawnedBomb.GetComponent <CircleCollider2D>();
        spriteRenderer.enabled = true;
    }
    protected override void StartFiring()
    {
        base.StartFiring();
        //Enable collider
        if(spawnedCollider != null)
        {
            spawnedCollider.enabled = true;
        }
        


    }

    protected override void StartWinddown()
    {
        base.StartWinddown();
        //Disable collider
        if(spawnedCollider != null)
        {
            Destroy(spawnedCollider.gameObject);
        }
       
    }
}
