using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Bomb : Ability_Simple
{
    //Used to make sure the inital collider is off.
    private CircleCollider2D bombCollider;
    //Bomb Sprite.
    private SpriteRenderer spriteRenderer;
    //Collider that will damage enemies.
    private CircleCollider2D spawnedCollider;
    //How we will apply force to the bomb.
    private Rigidbody2D bombRigidBody;
    //The amount of force to apply.
    private float bombLaunchSpeed = 1000f;
    //Ensure we only apply this force once.
    private bool doOnce = true;

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
        //Seems like sometimes the impulse would increase as if the button was fired more than once.
        //This is to attempt to fix that bug.
        doOnce = true;

        //Enable collider
        if(spawnedCollider != null)
        {
            
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, spawnedCollider.radius, Vector2.zero);
            Debug.Log(hit.collider.gameObject.name);
            StartCoroutine(TurnOffCollider());
        }
           

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
        //Disable collider and destroy the dead bomb
        if(spawnedCollider != null)
        {
            StopAllCoroutines();
            Destroy(spawnedCollider.gameObject);
        }
       
    }

    private IEnumerator TurnOffCollider()
    {
        yield return new WaitForSeconds(0.3f);
        spawnedCollider.enabled = false;
    }
}
