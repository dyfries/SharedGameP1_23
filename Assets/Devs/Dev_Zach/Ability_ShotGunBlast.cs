using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ability_ShotGunBlast : Ability_Simple
{

    [Header("Now in Ability ShotGunBlast Subclass")]
    public Rigidbody2D projectile;     //rigidbody2d?
    public Transform startPoint;       //Vector2

    [SerializeField, Range(1f, 45f)]
    public float turnAngle = 30.0f;

    [Header("ONLY DO 3, 5, or 7")]
    [SerializeField, Range(3, 7)]
    private int bulletAmount = 3;
    [SerializeField, Range(1f, 50f)]
    public float bulletForce = 10f;

    private SpriteRenderer windupRenderer;
    private Sprite noneSprite;
    public Sprite[] windupSprites;

    private float windupTimer;

    protected void Start()
    {
        if (projectile == null)
        {
            Debug.LogWarning("no projectile");
            enabled = false;
        }

        windupRenderer = GetComponent<SpriteRenderer>();

        if(windupRenderer == null )
        {
            Debug.Log("no spriteRenderer");
            windupRenderer.enabled = false;
        }

        if(windupSprites == null)
        {
            Debug.Log("nothing in sprite array");
            //windupSprites.enabled = false;
        }
                               //allBullets = new Rigidbody2D[bulletAmount];
    }

    protected override void Update()
    {
        base.Update();

        if(stageOfAbility == StageOfAbility.windup)
        {
            windupTimer += Time.deltaTime / activatedAbility_WindupTimer;
            Debug.Log(windupTimer);
            if(windupTimer > 0 && windupTimer < activatedAbility_WindupTimer/2)
            {
                windupRenderer.sprite = windupSprites[0];
            }

            if (windupTimer > activatedAbility_WindupTimer/2 && windupTimer < activatedAbility_WindupTimer)
            {
                windupRenderer.sprite = windupSprites[1];
            }

            //windupRenderer.sprite = windup1;
            /*if (windupTimer >= activatedAbility_WindupTimer)
            {
                //windupRenderer.sprite = null;
                //windupRenderer.sprite = windupSprites[-1];
                windupRenderer.sprite = windup1;
                windupTimer = 0;
            }*/
        }
        if(stageOfAbility == StageOfAbility.firing) 
        {
            windupRenderer.sprite = noneSprite;
            windupTimer = 0;
        }
    }

    protected override void StartWindup()
    {
        base.StartWindup();
        
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

    protected override void StartCooldown()
    {
        base.StartCooldown();

    }



}
