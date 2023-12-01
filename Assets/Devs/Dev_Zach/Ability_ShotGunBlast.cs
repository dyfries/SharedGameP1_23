using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_ShotGunBlast : Ability_Simple
{

    [Header("Now in Ability ShotGunBlast Subclass")]
    public Rigidbody2D projectile;     //rigidbody2d?
    public Transform startPoint;       //Vector2

    public Rigidbody2D[] allBullets;// only one bullet obj to be able to use


    public float turnAngle = 30.0f;

    [Header("ONLY DO 3, 5, or 7")]
    [SerializeField, Range(3, 7)]
    private int bulletAmount = 3;
    public float bulletForce = 10f;

    private SpriteRenderer windupRenderer;
    public Sprite windup1;

    protected void Start()
    {
        if (projectile == null)
        {
            Debug.LogWarning("no projectile");
            enabled = false;
        }

        windupRenderer = GetComponent<SpriteRenderer>();

                               //allBullets = new Rigidbody2D[bulletAmount];
    }

    protected override void StartWindup()
    {
        base.StartWindup();

        windupRenderer.sprite = windup1;

    }

    protected override void StartFiring()
    {

        base.StartFiring();

        // Add projectiles
              
        Rigidbody2D firstBullet = Instantiate(projectile, startPoint);          
        firstBullet.SetRotation( Quaternion.identity );
        firstBullet.AddForce(Vector2.up * bulletForce, ForceMode2D.Impulse);
        for (int i = 1; i < bulletAmount/2+.5; i++)
        {
            Rigidbody2D newBulletRight = Instantiate(projectile, startPoint);     

            newBulletRight.SetRotation(Quaternion.Euler(0, 0, -turnAngle * i));
            newBulletRight.AddRelativeForce(Vector2.up * bulletForce, ForceMode2D.Impulse);

            Rigidbody2D newBulletLeft = Instantiate(projectile, startPoint);  
            newBulletLeft.SetRotation(Quaternion.Euler(0, 0, turnAngle * i));
            newBulletLeft.AddRelativeForce(Vector2.up * bulletForce, ForceMode2D.Impulse);

            
            
        }

    }

}
