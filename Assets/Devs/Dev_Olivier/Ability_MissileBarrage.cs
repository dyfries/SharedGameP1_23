using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_MissileBarrage : Ability_Simple
{
    [Header("Missile Barrage Settings")]
    [SerializeField] private Rigidbody2D missile;
    [SerializeField] private int amountOfMissiles = 5;
    [SerializeField] private float missileForce = 10;
    private Rigidbody2D[] missiles;

    protected override void StartWindup()
    {
        base.StartWindup();

        missiles = new Rigidbody2D[amountOfMissiles];

        for (int i = 0; i < amountOfMissiles; i++)
        {

            //centers line of missile above player
            float xPos = (i*missile.transform.localScale.x) - ((float)amountOfMissiles/2*missile.transform.localScale.x) + (missile.transform.localScale.x/2);
            Vector2 spawnPosition = new Vector2(transform.position.x + xPos, transform.position.y + (1-Mathf.Abs(xPos)));

            Rigidbody2D newMissile = Instantiate(missile, spawnPosition, Quaternion.identity);
            missiles[i] = newMissile;
        }

    }

    protected override void StartFiring()
    {
        base.StartFiring();

        foreach (Rigidbody2D rb in missiles)
        {
            rb.AddForce(Vector2.up * missileForce, ForceMode2D.Impulse);
        }

    }

}
