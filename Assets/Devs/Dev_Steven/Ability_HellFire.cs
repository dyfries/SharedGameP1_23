using System.Collections.Generic;
using UnityEngine;

public class Ability_HellFire : Ability_Simple
{
    [Header("References & Layers")]
    private SpriteRenderer playerSpriteRenderer;
    public GameObject projectilePrefab;
    public GameObject explosionPrefab;
    public GameObject crosshairPrefab;
    public LayerMask NPCLayers;

    [Header("Ability Settings")]
    [Range(1, 100)]
    public int projectileCount = 9;
    [Range(0.1f, 10f)]
    public float launchSpeed = 2f;
    [Range(0.1f, 10f)]
    public float targetSpeed = 4f;
    [Range(0f, 8f)]
    public float targetDistance = 4f;
    private Vector3 crosshairPosition;
    [Range(0f, 500f)]
    public float rotationSpeed = 150f;
    [Range(0f, 500f)]
    public float lockOnRotationSpeed = 200f;
    [Range(0.01f, 5f)]
    public float detectionRange = 2f;
    [Range(0.01f, 5f)]
    public float destroyRange = 0.2f;
    [Range(0.1f, 5f)]
    public float explosionDestroyTime = 1.5f;

    [Header("Ability Toggles")]
    public bool randomSpawnAngle = false;
    private float angleStep;
    public bool showCrosshair = true;
    public bool destroyProjectileOnCollision = true;
    public bool destroyNPCOnCollision = true;
    public lockOn lockOnSetting;
    public enum lockOn
    {
        DisableLockOn, RayCastLockOn, OverlapCircleLockOn, 
    }

    [Header("Audio Sources")]
    public AudioSource readySound;
    public AudioSource windupSound;
    public AudioSource firingSound;
    public AudioSource winddownSound;
    public AudioSource cooldownSound;

    public AudioSource launchSound;
    public AudioSource explosionSound;

    public List<GameObject> projectiles;
    public List<Vector3> targetPositions;

    private void Awake()
    {
        playerSpriteRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>();
        if (playerSpriteRenderer == null) {
            Debug.LogError(gameObject.name + " can't locate SpriteRenderer in children of the parent object.");
        }

        if (readySound == null) {
            Debug.Log(gameObject.name + " is missing a reference to readySound. Please set one in the inspector for this effect.");
        }
        if (windupSound == null) {
            Debug.Log(gameObject.name + " is missing a reference to windupSound. Please set one in the inspector for this effect.");
        }
        if (firingSound == null) {
            Debug.Log(gameObject.name + " is missing a reference to firingSound. Please set one in the inspector for this effect.");
        }
        if (winddownSound == null) {
            Debug.Log(gameObject.name + " is missing a reference to winddownSound. Please set one in the inspector for this effect.");
        }
        if (cooldownSound == null) {
            Debug.Log(gameObject.name + " is missing a reference to cooldownSound. Please set one in the inspector for this effect.");
        }
        if (launchSound == null) {
            Debug.Log(gameObject.name + " is missing a reference to launchSound. Please set one in the inspector for this effect.");
        }
        if (explosionSound == null) {
            Debug.Log(gameObject.name + " is missing a reference to explosionSound. Please set one in the inspector for this effect.");
        }
    }

    protected void Start()
    {
        StartReady();
    }

    protected override void Update()
    {
        if (projectiles.Count > 0)
        {
            for (int i = 0; i < projectiles.Count; i ++)
            {
                if (projectiles[i] != null && targetPositions[i] != null)
                {
                    if (stageOfAbility == StageOfAbility.firing || stageOfAbility == StageOfAbility.winddown)
                    {            
                        TargetRangeDetection(i);
                        if (lockOnSetting != lockOn.DisableLockOn)
                        {
                            NPCLockOnDetection(i);
                        }
                        NPCHitDetection(i);
                    }

                    if (stageOfAbility == StageOfAbility.firing)
                    {
                        projectiles[i].transform.position = projectiles[i].transform.position + launchSpeed * Time.deltaTime * projectiles[i].transform.up;
                    }

                    if (stageOfAbility == StageOfAbility.winddown)
                    {            
                        Vector3 rotateTowards = (targetPositions[i] - projectiles[i].transform.position).normalized;
                        float smoothRotateAngle = Vector2.SignedAngle(projectiles[i].transform.up, rotateTowards);

                        if (smoothRotateAngle > 0.1f)
                        {
                            if (targetPositions[i] != crosshairPosition)
                            {
                                projectiles[i].transform.Rotate(0f, 0f, lockOnRotationSpeed * Time.deltaTime);
                            }
                            else
                            {
                                projectiles[i].transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
                            }
                        }
                        else if (smoothRotateAngle < -0.1f)
                        {
                            if (targetPositions[i] != crosshairPosition)
                            {
                                projectiles[i].transform.Rotate(0f, 0f, -lockOnRotationSpeed * Time.deltaTime);
                            }
                            else
                            {
                                projectiles[i].transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
                            }
                        }

                        projectiles[i].transform.position = projectiles[i].transform.position + targetSpeed * Time.deltaTime * projectiles[i].transform.up;
                    }
                }
            }
        }

        base.Update();
    }

    protected override void StartReady()
    {
        playerSpriteRenderer.color = Color.white;

        if (readySound != null)
        {
            readySound.Play();
        }

        projectiles.Clear();
        targetPositions.Clear();

        base.StartReady();
    }

    protected override void StartWindup()
    {
        playerSpriteRenderer.color = Color.green;

        if (windupSound != null)
        {
            windupSound.Play();
        }

        crosshairPosition = transform.position + Vector3.up * targetDistance;

        for (int i = 0; i < projectileCount; i++)
        {
            Vector3 target = crosshairPosition;
            targetPositions.Add(target);
        }

        if (showCrosshair)
        {
            if (crosshairPrefab != null)
            {
                GameObject crosshair = Instantiate(crosshairPrefab, crosshairPosition, Quaternion.identity);
                Destroy(crosshair, activatedAbility_WindupTimer + activatedAbility_FiringTimer + activatedAbility_WinddownTimer);
            }
            else
            {
                Debug.LogError(gameObject.name + " is missing a reference to a crosshair prefab.");
            }
        }

        base.StartWindup();
    }

    protected override void StartFiring()
    {
        playerSpriteRenderer.color = Color.grey;

        angleStep = 360f / projectileCount;

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = i * angleStep;
            Quaternion rotation;
            if (randomSpawnAngle) 
            {
                rotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360));
            }
            else 
            {
                rotation = Quaternion.Euler(0f, 0f, angle);
            }

            GameObject projectile = Instantiate(projectilePrefab, transform.position, rotation, gameObject.transform);
            projectile.name = "Projectile " + i;
            projectiles.Add(projectile);

            if (launchSound != null)
            {
                launchSound.Play();
            }
        }

        base.StartFiring();
    }

    protected override void StartWinddown()
    {
        playerSpriteRenderer.color = Color.red;

        if (winddownSound != null)
        {
            winddownSound.Play();
        }

        for (int i = 0; i < projectiles.Count; i++)
        {
            if (projectiles[i] != null)
            {
                if (firingSound != null)
                {
                    firingSound.Play();
                }

                ParticleSystem particles = projectiles[i].GetComponentInChildren<ParticleSystem>();

                if (particles != null)
                {
                    particles.Play();
                }
            }
        }

        base.StartWinddown();
    }

    protected override void StartCooldown()
    {
        playerSpriteRenderer.color = Color.magenta;

        if (cooldownSound != null)
        {
            cooldownSound.Play();
        }

        for (int i = 0; i < projectiles.Count; i++)
        {
            if (projectiles[i] != null)
            {
                DestroyProjectile(i);
            }
        }

        projectiles.Clear();
        targetPositions.Clear();

        base.StartCooldown();
    }

    private void TargetRangeDetection(int i)
    {
        float distanceFromTarget = Vector2.Distance(crosshairPosition, projectiles[i].transform.position);

        if (distanceFromTarget < 0.3f)
        {
            DestroyProjectile(i);
        }
    }

    private void NPCLockOnDetection(int i)
    {
        SpriteRenderer projectileSprite = projectiles[i].GetComponentInChildren<SpriteRenderer>();
        if (projectileSprite == null)
        {
            Debug.Log(gameObject.name + "'s Projectile Prefab is missing a SpriteRenderer");
            return;
        }

        if (lockOnSetting == lockOn.RayCastLockOn)
        {
            RaycastHit2D detectionRay = Physics2D.Raycast(projectiles[i].transform.position, projectiles[i].transform.up, detectionRange, NPCLayers);

            if (detectionRay.collider != null)
            {
                targetPositions[i] = detectionRay.collider.transform.position;
                projectileSprite.color = Color.red;
                return;
            }

            targetPositions[i] = crosshairPosition;
            projectileSprite.color = Color.white;
        }
        else if (lockOnSetting == lockOn.OverlapCircleLockOn)
        {
            Collider2D[] detectionCircle = Physics2D.OverlapCircleAll(projectiles[i].transform.position, detectionRange, NPCLayers);

            for (int j = 0; j < detectionCircle.Length; j++)
            {
                if (detectionCircle[j] != null)
                {
                    targetPositions[i] = detectionCircle[j].transform.position;
                    projectileSprite.color = Color.red;
                    return;
                }
            }

            targetPositions[i] = crosshairPosition;
            projectileSprite.color = Color.white;
        }
    }

    private void NPCHitDetection(int i)
    {
        RaycastHit2D hitRay;
        hitRay = Physics2D.Raycast(projectiles[i].transform.position, projectiles[i].transform.up, destroyRange, NPCLayers);

        if (hitRay.collider != null)
        {
            if (destroyProjectileOnCollision)
            {
                DestroyProjectile(i);
            }             

            if (destroyNPCOnCollision)
            {
                Destroy(hitRay.collider.gameObject);
            }
        }
    }

    private void DestroyProjectile(int i)
    {
        if (explosionSound != null)
        {
            explosionSound.Play();
        }

        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, projectiles[i].transform.position, projectiles[i].transform.rotation);
            Destroy(explosion, explosionDestroyTime);
        }
        else
        {
            Debug.Log(name + " is missing a reference to an explosion prefab. Please set one in the inspector for this effect.");
        }

        Destroy(projectiles[i]);
    }

    private void OnDrawGizmos()
    {
        if (DEBUG_MODE)
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (projectiles[i] != null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawRay(projectiles[i].transform.position, projectiles[i].transform.up * destroyRange);
                    
                    if (lockOnSetting == lockOn.OverlapCircleLockOn)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawWireSphere(projectiles[i].transform.position, detectionRange);
                    }
                    else if (lockOnSetting == lockOn.RayCastLockOn)
                    {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawRay(projectiles[i].transform.position, projectiles[i].transform.up * detectionRange);
                    }
                }
            }
        }
    }
}