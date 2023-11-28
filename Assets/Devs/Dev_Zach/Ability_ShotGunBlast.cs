using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_ShotGunBlast : Ability_Simple
{

    [Header("Now in Ability ShotGunBlast Subclass")]
    public Rigidbody2D projectile; //rigidbody2d?
    public Vector2 startPoint;

    public Rigidbody2D[] allBullets;


    public float turnAngle = 30.0f;

    [Header("ONLY DO 3, 5, or 7")]
    [SerializeField, Range(3, 7)]
    private int bulletAmount = 3;
    public float bulletForce = 10f;

    protected void Start()
    {
        if (projectile == null)
        {
            Debug.LogWarning("no projectile");
            enabled = false;
        }

        //allBullets = new Rigidbody2D[bulletAmount];
    }

    protected override void StartFiring()
    {

        base.StartFiring();

        // Add projectiles
        /*GameObject newProjectile = */
        Rigidbody2D firstBullet = Instantiate(projectile, startPoint, Quaternion.identity);
        firstBullet.AddForce(Vector2.up * bulletForce, ForceMode2D.Impulse);
        for (int i = 1; i < bulletAmount/2+.5; i++)
        {
                Rigidbody2D newBulletRight = Instantiate(projectile, startPoint, Quaternion.Euler(0, 0, -turnAngle * i));
                newBulletRight.AddRelativeForce(Vector2.up * bulletForce, ForceMode2D.Impulse);

                Rigidbody2D newBulletLeft = Instantiate(projectile, startPoint, Quaternion.Euler(0,0,turnAngle * i));
                newBulletLeft.AddRelativeForce(Vector2.up * bulletForce, ForceMode2D.Impulse);

            
            
        }
        
        /*Instantiate(projectile, startPoint);
        Instantiate(projectile, startPoint);*/


    }

}
