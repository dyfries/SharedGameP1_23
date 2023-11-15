using System.Collections.Generic;
using UnityEngine;

public class Ability_HellFire : Ability_Simple
{
    private SpriteRenderer playerSpriteRenderer;

    public GameObject projectilePrefab;
    public GameObject explosionPrefab;

    public int projectileCount = 10;
    public float projectileSpeed = 2f;
    public float rotationSpeed = 2f;
    private float angleStep;

    public List<GameObject> projectiles;

    private Vector3 targetPosition;
    public GameObject crosshairPrefab;


    private void Awake()
    {
        playerSpriteRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>();
        if (playerSpriteRenderer == null)
        {
            Debug.LogError(gameObject.name + " can't locate SpriteRenderer in children of parent object.");
        }
    }

    protected void Start()
    {
        StartReady();
    }

    protected override void Update()
    {
        if (stageOfAbility == StageOfAbility.firing)
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (projectiles[i] != null)
                {
                    Vector3 rotateTowards = (targetPosition - projectiles[i].transform.position).normalized;
                    float smoothRotateAngle = Vector2.SignedAngle(projectiles[i].transform.up, rotateTowards);

                    if (smoothRotateAngle > 0)
                    {
                        // Rotate Left
                        projectiles[i].transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
                    }
                    else if (smoothRotateAngle < 0)
                    {
                        // Rotate Right
                        projectiles[i].transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
                    }
                }
            }
        }

        if (stageOfAbility == StageOfAbility.windup 
            || stageOfAbility == StageOfAbility.firing 
            || stageOfAbility == StageOfAbility.winddown)
        {
            for (int i = 0; i < projectiles.Count; i++)
            {               
                if (projectiles[i] != null)
                {
                    //projectiles[i].GetComponent<Rigidbody2D>().AddForce(projectiles[i].transform.up * projectileSpeed, ForceMode2D.Impulse);
                    projectiles[i].transform.position = projectiles[i].transform.position + projectiles[i].transform.up * projectileSpeed * Time.deltaTime;                    
                }
            }
        }

        for (int i = 0; i < projectiles.Count; i++)
        {
            if (projectiles[i] != null)
            {
                float distanceFromTarget = Vector2.Distance(targetPosition, projectiles[i].transform.position);

                if (distanceFromTarget < 0.3f)
                {
                    Destroy(projectiles[i]);

                    GameObject explosion;
                    explosion = Instantiate(explosionPrefab, projectiles[i].gameObject.transform.position, projectiles[i].gameObject.transform.rotation);
                    Destroy(explosion, 1.5f);
                }
            }
        }

        base.Update();
    }

    protected override void StartReady()
    {
        playerSpriteRenderer.color = Color.white;        

        base.StartReady();
    }

    protected override void StartWindup()
    {
        playerSpriteRenderer.color = Color.green;

        targetPosition = transform.position + Vector3.up * 3f;

        GameObject crosshair;
        crosshair = Instantiate(crosshairPrefab, targetPosition, Quaternion.identity);
        Destroy(crosshair, 3f);

        for (int i = 0; i < projectileCount; i++)
        {
            angleStep = 360f / projectileCount;
            float angle = i * angleStep;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

            GameObject projectile;
            //projectile = Instantiate(projectilePrefab, transform.position, Random.rotation, gameObject.transform);
            projectile = Instantiate(projectilePrefab, transform.position, rotation, gameObject.transform);
            projectile.name = "Projectile " + i;
            projectiles.Add(projectile);
        }        

        base.StartWindup();
    }

    protected override void StartFiring()
    {
        playerSpriteRenderer.color = Color.red;


        for (int i = 0; i < projectiles.Count; i++)
        {
            //projectiles[i].GetComponent<Rigidbody2D>().AddForce(projectiles[i].transform.up * projectileSpeed, ForceMode2D.Impulse);
        }

        base.StartFiring();
    }

    protected override void StartWinddown()
    {
        playerSpriteRenderer.color = Color.grey;

        for (int i = 0; i < projectiles.Count; i++)
        {
            if (projectiles[i] != null)
            {
                Destroy(projectiles[i]);

                GameObject explosion;
                explosion = Instantiate(explosionPrefab, projectiles[i].gameObject.transform.position, projectiles[i].gameObject.transform.rotation);
                Destroy(explosion, 1.5f);
            }
        }

        base.StartWinddown();
    }

    protected override void StartCooldown()
    {
        playerSpriteRenderer.color = Color.magenta;

        projectiles.Clear();

        for (int i = 0; i < projectiles.Count; i++)
        {

        }

        base.StartCooldown();
    }
}