using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_ShotGunBlast : Ability_Simple
{

    [Header("Now in Ability ShotGunBlast Subclass")]
    public Rigidbody2D projectile; //rigidbody2d?
    public Transform startPoint;

    public Rigidbody2D[] allBullets;


    public float turnAngle = 30.0f;

    [Header("ONLY DO 3, 5, or 7")]
    [SerializeField, Range(3, 7)]
    private int bulletAmount = 3;

    protected void Start()
    {
        if (projectile == null)
        {
            Debug.LogWarning("no projectile");
            enabled = false;
        }

        allBullets = new Rigidbody2D[bulletAmount];
    }

    protected override void StartFiring()
    {

        base.StartFiring();

        // Add projectiles
        /*GameObject newProjectile = */
        for (int i = 0; i < bulletAmount; i++)
        {
            Rigidbody2D newBullet = Instantiate(projectile, startPoint);
        }
        
        /*Instantiate(projectile, startPoint);
        Instantiate(projectile, startPoint);*/


    }

}
