using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Ability_Bomb : Ability_Simple
{
    [Header("Particle Systems")]
    //Used to play the particle system of the bomb.
    public ParticleSystem[] bombVFX;
    //Bomb Sprite.
    private SpriteRenderer spriteRenderer;
    //How we will apply force to the bomb.
    private Rigidbody2D bombRigidBody;
    //The amount of force to apply.
    private float bombLaunchSpeed = 500f;
    //Ensure we only apply this force once.
    private bool doOnce = true;
    //Bomb
    private GameObject spawnedBomb;
    //Used gizmos to see that 5f is the exact radius.
    //Also, the radius is set in particle system.
    private float bombRadius = 5f;

    protected override void StartWindup()
    {
        base.StartWindup();

        //Instansiate Projectile
        if (doOnce)
        {
            doOnce = false;
            SpawnBomb();
        }

        //Set the bomb sprite to be black and turn it on.
        BombSpriteSettings();
        //Play the inital fire trail.
        PlayParticleSystem(0);
    }

    
    protected override void StartFiring()
    {
        base.StartFiring();
        //Seems like sometimes the impulse would increase as if the button was fired more than once.
        //This is to attempt to fix that bug.
        doOnce = true;

        //Collect our hits.
        if (spawnedBomb != null)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(spawnedBomb.transform.position, bombRadius, Vector2.zero);
            
            foreach (var hit in hits)
            {
                //Do something with hits.
                Debug.Log(hit.rigidbody.gameObject.name);
            }
        }
        
        //Stop projectile from moving
        if(bombRigidBody != null)
            bombRigidBody.velocity = Vector2.zero;

        //Turn off the bomb sprite
        if(spriteRenderer != null)
            spriteRenderer.enabled = false;
        //Turn on the Explosion animation
        PlayParticleSystem(1);
    }
       

    protected override void StartWinddown()
    {
        base.StartWinddown();

        //Play smoke particle system.
        PlayParticleSystem(2);
    }

    protected override void StartCooldown()
    {
        base.StartCooldown();

        //Destroy bomb instance
        if (spawnedBomb != null)
        {
            Destroy(spawnedBomb.gameObject);
        }
    }

    //Sets particle system to active and then plays it.
    private void PlayParticleSystem(int index)
    {
        if (bombVFX != null && bombVFX.Length != 0)
        {
            foreach (var vfx in bombVFX)
            {
                if (vfx == null)
                {
                    return;
                }
                else
                {
                    bombVFX[index].gameObject.SetActive(true);
                    bombVFX[index].Play();
                }
            }

        }
    }

    //Gets the sprite, turns it on, and colors it black.
    private void BombSpriteSettings()
    {
        spriteRenderer = spawnedBomb.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        spriteRenderer.color = Color.black;
    }

    //Instansiating our bomb
    //Applys an impulse force to it.
    private void SpawnBomb()
    {
        Vector3 spawnPos = transform.parent.GetComponent<Transform>().position;
        spawnedBomb = Instantiate(this.gameObject, spawnPos, Quaternion.identity);
        bombRigidBody = spawnedBomb.GetComponent<Rigidbody2D>();
        bombRigidBody.AddForce(Vector2.up * bombLaunchSpeed * Time.deltaTime, ForceMode2D.Impulse);
        bombVFX = spawnedBomb.GetComponentsInChildren<ParticleSystem>(true);
    }

}
