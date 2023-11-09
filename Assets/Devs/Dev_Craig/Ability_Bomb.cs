using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Bomb : Ability_Simple
{
    private CircleCollider2D bombCollider;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D spawnedCollider;
    private Rigidbody2D bombRigidBody;
    private float bombLaunchSpeed = 1500f;
    private bool doOnce = true;

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
        GameObject spawnedBomb = Instantiate(this.gameObject, transform.parent.GetComponent<Transform>().position, Quaternion.identity);
        bombRigidBody = spawnedBomb.GetComponent<Rigidbody2D>();

        if (doOnce)
        {
            doOnce = false;
            bombRigidBody.AddForce(Vector2.up * bombLaunchSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }

        spriteRenderer = spawnedBomb.GetComponent<SpriteRenderer>();
        spawnedCollider = spawnedBomb.GetComponent <CircleCollider2D>();
        spriteRenderer.enabled = true;
    }
    protected override void StartFiring()
    {
        base.StartFiring();
        doOnce = true;
        //Enable collider
        if(spawnedCollider != null)
            spawnedCollider.enabled = true;

        //Stop projectile from moving
        if(bombRigidBody != null)
            bombRigidBody.velocity = Vector2.zero;

        //Turn off the bomb sprite
        if(spriteRenderer != null)
            spriteRenderer.enabled = false;
        //Turn on the Explosion animation
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
