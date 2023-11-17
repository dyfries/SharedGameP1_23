using System.Collections.Generic;
using UnityEngine;

public class Ability_HellFire : Ability_Simple
{
    private SpriteRenderer playerSpriteRenderer;

    [Header("Prefab References")]
    public GameObject projectilePrefab;
    public GameObject explosionPrefab;
    public GameObject crosshairPrefab;

    [Header("Ability Settings")]
    [Range(1, 100)]
    public int projectileCount = 9;
    [Range(0.1f, 5f)]
    public float launchSpeed = 2f;
    [Range(0.1f, 5f)]
    public float targetSpeed = 4f;
    [Range(2f, 10f)]
    public float targetDistance = 4f;
    private Vector3 targetPosition;
    [Range(0f, 300f)]
    public float rotationSpeed = 150f;
    private float angleStep;

    public List<GameObject> projectiles;

    [Header("Audio Sources")]
    public AudioSource readySound;
    public AudioSource windupSound;
    public AudioSource firingSound;
    public AudioSource winddownSound;
    public AudioSource cooldownSound;

    public AudioSource launchSound;
    public AudioSource explosionSound;

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
        /*if (stageOfAbility == StageOfAbility.windup)
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (projectiles[i] != null)
                {

                }
            }
        }*/

        if (stageOfAbility == StageOfAbility.firing)
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (projectiles[i] != null)
                {
                    //projectiles[i].GetComponent<Rigidbody2D>().AddForce(projectiles[i].transform.up * projectileSpeed, ForceMode2D.Impulse);
                    projectiles[i].transform.position = projectiles[i].transform.position + launchSpeed * Time.deltaTime * projectiles[i].transform.up;
                }
            }
        }

        if (stageOfAbility == StageOfAbility.winddown)
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

                    projectiles[i].transform.position = projectiles[i].transform.position + targetSpeed * Time.deltaTime * projectiles[i].transform.up;                    
                }
            }
        }

        for (int i = 0; i < projectiles.Count; i++)
        {
            if (projectiles[i] != null)
            {
                Physics2D.Raycast(projectiles[i].transform.position, projectiles[i].transform.up, 3f);
                Debug.DrawRay(projectiles[i].transform.position, projectiles[i].transform.up * 3f, Color.red);


                float distanceFromTarget = Vector2.Distance(targetPosition, projectiles[i].transform.position);

                if (distanceFromTarget < 0.3f)
                {
                    Destroy(projectiles[i]);

                    explosionSound.Play();

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
        readySound.Play();

        base.StartReady();
    }

    protected override void StartWindup()
    {
        playerSpriteRenderer.color = Color.green;
        windupSound.Play();

        targetPosition = transform.position + Vector3.up * targetDistance;

        GameObject crosshair;       
        crosshair = Instantiate(crosshairPrefab, targetPosition, Quaternion.identity);
        Destroy(crosshair, activatedAbility_WindupTimer + activatedAbility_FiringTimer + activatedAbility_WinddownTimer);


        base.StartWindup();
    }

    protected override void StartFiring()
    {
        playerSpriteRenderer.color = Color.red;       

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

            launchSound.Play();
        }

        for (int i = 0; i < projectiles.Count; i++)
        {
            if (projectiles[i] != null)
            {

            }
        }

        base.StartFiring();
    }

    protected override void StartWinddown()
    {
        playerSpriteRenderer.color = Color.grey;
        winddownSound.Play();

        for (int i = 0; i < projectiles.Count; i++)
        {
            if (projectiles[i] != null)
            {
                firingSound.Play();
            }
        }

        base.StartWinddown();
    }

    protected override void StartCooldown()
    {
        playerSpriteRenderer.color = Color.magenta;
        cooldownSound.Play();

        for (int i = 0; i < projectiles.Count; i++)
        {
            if (projectiles[i] != null)
            {
                Destroy(projectiles[i]);

                explosionSound.Play();

                GameObject explosion;
                explosion = Instantiate(explosionPrefab, projectiles[i].gameObject.transform.position, projectiles[i].gameObject.transform.rotation);
                Destroy(explosion, 1.5f);
            }
        }

        projectiles.Clear();

        base.StartCooldown();
    }
}