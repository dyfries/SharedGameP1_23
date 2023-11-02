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
    private Vector2 barrageOrigin;
    private float windupTimer = 0;

    protected override void Update()
    {
        base.Update();

        if(stageOfAbility == StageOfAbility.windup)
        {
            windupTimer += Time.deltaTime/activatedAbility_WindupTimer;

            for (int i = 0; i < amountOfMissiles; i++)
            {
                //centers arrow of missile above player
                float xPos = (i * missile.transform.localScale.x) - ((float)amountOfMissiles / 2 * missile.transform.localScale.x) + (missile.transform.localScale.x / 2);
                Vector2 spawnPosition = new Vector2(barrageOrigin.x + xPos, barrageOrigin.y + (1 - Mathf.Abs(xPos)));

                missiles[i].transform.position = Vector2.Lerp(barrageOrigin, spawnPosition, windupTimer);

            }
        }

    }

    protected override void StartWindup()
    {
        base.StartWindup();

        missiles = new Rigidbody2D[amountOfMissiles];

        barrageOrigin = transform.position;

        for (int i = 0; i < amountOfMissiles; i++)
        {
            //create missiles in scene and keep track of them
            Rigidbody2D newMissile = Instantiate(missile, barrageOrigin, Quaternion.identity);
            newMissile.GetComponent<Collider2D>().enabled = false;
            missiles[i] = newMissile;
        }

    }

    protected override void StartFiring()
    {
        base.StartFiring();

        StartCoroutine(StageredFire());

        //old code launches missles all at same time
        //
        //foreach (Rigidbody2D rb in missiles)
        //{
        //    rb.AddForce(Vector2.up * missileForce, ForceMode2D.Impulse);
        //    rb.GetComponent<Collider2D>().enabled = true;
        //}

    }

    IEnumerator StageredFire()
    {
        //staggers when missiles are fired for coolness points
        for (int i = 0; i < amountOfMissiles; i++)
        {
            int missileToGet = i;
            if(i % 2 == 1)
            {
                missileToGet = (amountOfMissiles - i);
            }
            //calculate how long to wait based on how many missles we need to fire in how much time
            yield return new WaitForSeconds(activatedAbility_FiringTimer / amountOfMissiles);
            missiles[missileToGet].AddForce(Vector2.up * missileForce, ForceMode2D.Impulse);
            missiles[missileToGet].GetComponent<Collider2D>().enabled = true;
        }
    }

}
