using System.Collections.Generic;
using UnityEngine;

// The HellFire ability inherits from the Ability_Simple class
public class Ability_HellFire : Ability_Simple
{
    [Header("Ability Settings")]
    [Range(1, 100)]
    public int projectileCount = 9;                     // Number of projectiles.
    [Range(0.1f, 20f)]
    public float launchSpeed = 2f;                      // Speed of projectile launch.
    [Range(0.1f, 20f)]
    public float targetSpeed = 4f;                      // Speed of projectile targeting movement.
    public Vector2 targetDistance;                      // Position of the crosshair from the player.
    private Vector3 crosshairPosition;                  // Current position of the crosshair.
    [Range(0f, 1000f)]
    public float rotationSpeed = 150f;                  // Base speed of projectile rotation.
    [Range(0f, 1000f)]
    public float lockOnRotationSpeed = 200f;            // Speed of lock-on rotation.
    [Range(0.01f, 5f)]
    public float detectionRange = 2f;                   // Range for NPC lock-on detection.
    [Range(0.01f, 5f)]
    public float destroyRange = 0.2f;                   // Range for NPC hit detection.
    [Range(0.1f, 5f)]
    public float explosionDestroyTime = 1.5f;           // Time for the explosion effect to destroy itself.

    [Header("Ability Toggles")]
    public bool randomSpawnAngle = false;               // Toggle for random projectile spawn angles.
    private float angleStep;                            // Angle step between projectiles.
    public bool showCrosshair = true;                   // Toggle to show the crosshair.
    public bool destroyProjectileOnCollision = true;    // Toggle for destroying projectiles on NPC collision.
    public bool destroyNPCOnCollision = true;           // Toggle for destroying NPCs on collision.
    public lockOn lockOnSetting;                        // Enum for lock-on settings.
    public enum lockOn
    {
        DisableLockOn, RayCastLockOn, OverlapCircleLockOn, 
    }

    [Header("References & Layers")]
    public GameObject projectilePrefab;                 // Prefab for the projectile.
    public GameObject crosshairPrefab;                  // Prefab for the crosshair.
    public GameObject explosionPrefab;                  // Prefab for the explosion effect.
    public GameObject projectileHolder;                 // GameObject to organize spawned projectiles
    public LayerMask NPCLayers;                         // Layers considered for NPC interactions.

    private SpriteRenderer playerSpriteRenderer;        // Reference to the player's sprite renderer.

    //[HideInInspector]
    private List<GameObject> projectiles;                // List to store instantiated projectiles.
    //[HideInInspector]
    private List<Vector3> targetPositions;               // List to store target positions for each projectile.

    [Header("Audio Sources")]
    public AudioSource readySound;                      // Sound played when the ability is ready.
    public AudioSource windupSound;                     // Sound played at windup state.
    public AudioSource firingSound;                     // Sound played at firing state.
    public AudioSource winddownSound;                   // Sound played at winddown state.
    public AudioSource cooldownSound;                   // Sound played at cooldown state.

    public AudioSource launchSound;                     // Sound played when projectiles are launched.
    public AudioSource explosionSound;                  // Sound played when projectile destroyed.


    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        // Locate SpriteRenderer in children of the parent object.
        playerSpriteRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>();

        projectiles = new List<GameObject>();
        targetPositions = new List<Vector3>();

        // Check and log missing references.
        ReferenceErrorCheck();
    }

    // Start is called before the first frame update.
    protected void Start()
    {
        // Start the ability in the "Ready" state.
        StartReady();
    }

    // Update is called once per frame.
    protected override void Update()
    {
        // Check if projectiles exist.

        if (projectiles.Count > 0)
        {
            // Iterate through each projectile.
            for (int i = 0; i < projectiles.Count; i ++)
            {
                // Check if the projectile and target position are not null.
                if (projectiles[i] != null && targetPositions[i] != null)
                {
                    // Check ability state and perform relevant actions.
                    if (stageOfAbility == StageOfAbility.firing || stageOfAbility == StageOfAbility.winddown)
                    {
                        // Check if the projectile is within target range.
                        CrosshairRangeDetection(i);
                        // Perform NPC lock-on detection if enabled.
                        if (lockOnSetting != lockOn.DisableLockOn)
                        {
                            NPCLockOnDetection(i);
                        }
                        // Perform NPC hit detection.
                        NPCHitDetection(i);
                    }

                    // Move projectile based on ability state.
                    if (stageOfAbility == StageOfAbility.firing)
                    {
                        // Move the projectile forward at launchSpeed.
                        projectiles[i].transform.position = projectiles[i].transform.position + launchSpeed * Time.deltaTime * projectiles[i].transform.up;
                    }

                    if (stageOfAbility == StageOfAbility.winddown)
                    {
                        // Calculate rotation angle towards the target position.
                        Vector3 rotateTowards = (targetPositions[i] - projectiles[i].transform.position).normalized;
                        float smoothRotateAngle = Vector2.SignedAngle(projectiles[i].transform.up, rotateTowards);

                        // Smoothly rotate the projectile based on the calculated angle.
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
                        // Move the projectile forward at targetSpeed.
                        projectiles[i].transform.position = projectiles[i].transform.position + targetSpeed * Time.deltaTime * projectiles[i].transform.up;
                    }
                }
            }
        }

        // Call the base class Update method.
        base.Update();
    }

    // Method to perform actions when ability is in "ready" state.
    protected override void StartReady()
    {
        // Play the ready sound if available.
        if (readySound != null)
        {
            readySound.Play();
        }

        // Clear projectile and target position lists.
        projectiles.Clear();
        targetPositions.Clear();

        // Call the base class StartReady method.
        base.StartReady();
    }

    // Method to perform actions when ability is in "windup" state.
    protected override void StartWindup()
    {
        // Play the windup sound if available.
        if (windupSound != null)
        {
            windupSound.Play();
        }

        // Calculate the position of the crosshair.
        crosshairPosition = transform.position + (Vector3)targetDistance;

        // Initialize target positions based on the crosshair.
        for (int i = 0; i < projectileCount; i++)
        {
            Vector3 target = crosshairPosition;
            // Add target positions to the list.
            targetPositions.Add(target);
        }

        // Check if showCrosshair toggle is enabled.
        if (showCrosshair)
        {
            // Check if crosshairPrefab is null
            if (crosshairPrefab != null)
            {
                // Instantiate and destroy the crosshair prefab.
                GameObject crosshair = Instantiate(crosshairPrefab, crosshairPosition, Quaternion.identity);
                Destroy(crosshair, activatedAbility_WindupTimer + activatedAbility_FiringTimer + activatedAbility_WinddownTimer);
            }
        }

        // Call the base class StartWindup method.
        base.StartWindup();
    }

    // Method to perform actions when ability is in "firing" state.
    protected override void StartFiring()
    {
        // Calculate the angle step between projectiles.
        angleStep = 360f / projectileCount;

        // Instantiate projectiles based on the specified count and angle step.
        for (int i = 0; i < projectileCount; i++)
        {
            // Calculate evenly distributed angles.
            float angle = i * angleStep;
            Quaternion rotation;

            // Set rotation based on randomSpawnAngle toggle.
            if (randomSpawnAngle) 
            {
                // Use a random spawn angle if randomSpawnAngle is true
                rotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360));
            }
            else 
            {
                // Use evenly distributed angles if randomSpawnAngle is false
                rotation = Quaternion.Euler(0f, 0f, angle);
            }

            // Check if the projectile is not null.
            if (projectilePrefab != null)
            {
                // Instantiate projectile, set its name, and add it to the list.
                GameObject projectile;
                if (projectileHolder != null)
                {
                    projectile = Instantiate(projectilePrefab, transform.position, rotation, projectileHolder.transform);
                }
                else
                {
                    projectile = Instantiate(projectilePrefab, transform.position, rotation);
                }
                projectile.name = "Projectile " + i;
                projectiles.Add(projectile);
            }

            // Play launch sound if available.
            if (launchSound != null)
            {
                launchSound.Play();
            }
        }

        // Call the base class StartFiring method.
        base.StartFiring();
    }

    // Method to perform actions when ability is in "winddown" state.
    protected override void StartWinddown()
    {
        // Play winddown sound if available.
        if (winddownSound != null)
        {
            winddownSound.Play();
        }

        // Iterate through projectiles
        for (int i = 0; i < projectiles.Count; i++)
        {
            // Check if the projectile is not null.
            if (projectiles[i] != null)
            {
                // Play the firing sound if available
                if (firingSound != null)
                {
                    firingSound.Play();
                }

                ParticleSystem particles = projectiles[i].GetComponentInChildren<ParticleSystem>();

                // Play particle effects if available
                if (particles != null)
                {
                    particles.Play();
                }
            }
        }

        // Call the base class StartWinddown method.
        base.StartWinddown();
    }

    // Method to perform actions when ability is in "cooldown" state.
    protected override void StartCooldown()
    {
        // Play the cooldown sound if available
        if (cooldownSound != null)
        {
            cooldownSound.Play();
        }

        // Iterate through remaining projectiles and destroy them.
        for (int i = 0; i < projectiles.Count; i++)
        {
            if (projectiles[i] != null)
            {
                DestroyProjectile(i);
            }
        }

        // Call the base class StartCooldown method.
        base.StartCooldown();
    }

    // Check if the projectile is within range of the Crosshair and destroy it if needed.
    private void CrosshairRangeDetection(int i)
    {
        // Calculate the distance between the crosshair position and the projectile.
        float distanceFromTarget = Vector2.Distance(crosshairPosition, projectiles[i].transform.position);

        // Check if the distance is less than the defined threshold for Crosshair range.
        if (distanceFromTarget < 0.3f)
        {
            // If within range, destroy the projectile.
            DestroyProjectile(i);
        }
    }

    // Perform NPC lock-on detection based on the lockOnSetting.
    private void NPCLockOnDetection(int i)
    {
        // Get the SpriteRenderer component of the projectile.
        SpriteRenderer projectileSprite = projectiles[i].GetComponentInChildren<SpriteRenderer>();

        // Check if the SpriteRenderer is missing in the Projectile Prefab.
        if (projectileSprite == null)
        {
            // Log an error if the SpriteRenderer is missing.
            Debug.Log(gameObject.name + "'s Projectile Prefab is missing a SpriteRenderer");
            return;
        }

        // Check the lock-on setting to determine the detection method.
        if (lockOnSetting == lockOn.RayCastLockOn)
        {
            // Perform a raycast to detect NPCs within the specified range.
            RaycastHit2D detectionRay = Physics2D.Raycast(projectiles[i].transform.position, projectiles[i].transform.up, detectionRange, NPCLayers);

            // Check if the ray hits a collider.
            if (detectionRay.collider != null)
            {
                // Update the target position to the detected NPC's position.
                targetPositions[i] = detectionRay.collider.transform.position;

                // Change the projectile's color to red for visual indication.
                projectileSprite.color = Color.red;
                return;
            }

            // If no NPC is detected, set the target position to the crosshair and change color to white.
            targetPositions[i] = crosshairPosition;
            projectileSprite.color = Color.white;
        }
        else if (lockOnSetting == lockOn.OverlapCircleLockOn)
        {
            // Perform an overlap circle check to detect NPCs within the specified range.
            Collider2D[] detectionCircle = Physics2D.OverlapCircleAll(projectiles[i].transform.position, detectionRange, NPCLayers);

            // Iterate through detected colliders in the circle.
            for (int j = 0; j < detectionCircle.Length; j++)
            {
                // Check if the OverLapCircleAll hits a collider.
                if (detectionCircle[j] != null)
                {
                    // Update the target position to the detected NPC's position.
                    targetPositions[i] = detectionCircle[j].transform.position;

                    // Change the projectile's color to red for visual indication.
                    projectileSprite.color = Color.red;
                    return;
                }
            }

            // If no NPC is detected, set the target position to the crosshair and change color to white.
            targetPositions[i] = crosshairPosition;
            projectileSprite.color = Color.white;
        }
    }

    // Perform NPC hit detection and take appropriate actions.
    private void NPCHitDetection(int i)
    {
        // Perform a 2D raycast from the projectile's position in the up direction to detect NPCs.
        RaycastHit2D hitRay = Physics2D.Raycast(projectiles[i].transform.position, projectiles[i].transform.up, destroyRange, NPCLayers);

        // Check if the ray hits a collider.
        if (hitRay.collider != null)
        {
            // Check if the option to destroy projectiles on collision is enabled.
            if (destroyProjectileOnCollision)
            {
                // Destroy the projectile if it collides with an NPC.
                DestroyProjectile(i);
            }

            // Check if the option to destroy NPCs on collision is enabled.
            if (destroyNPCOnCollision)
            {
                // Destroy the NPC GameObject that was hit by the ray.
                Destroy(hitRay.collider.gameObject);

                // Play explosion sound if available.
                if (explosionSound != null)
                {
                    explosionSound.Play();
                }

                // Instantiate and destroy the explosion effect.
                if (explosionPrefab != null)
                {
                    GameObject explosion = Instantiate(
                    explosionPrefab, hitRay.collider.gameObject.transform.position, hitRay.collider.gameObject.transform.rotation);
                    Destroy(explosion, explosionDestroyTime);
                }
            }
        }
    }

    private void DestroyProjectile(int i)
    {
        // Play explosion sound if available.
        if (explosionSound != null)
        {
            explosionSound.Play();
        }

        // Instantiate and destroy the explosion effect.
        if (explosionPrefab != null)
        {            
            GameObject explosion = Instantiate(explosionPrefab, projectiles[i].transform.position, projectiles[i].transform.rotation);
            Destroy(explosion, explosionDestroyTime);
        }

        // Destroy the projectile.
        Destroy(projectiles[i]);
    }

    // Check and log missing references.
    private void ReferenceErrorCheck()
    {
        // Check and log missing reference for the SpriteRenderer.
        if (playerSpriteRenderer == null)
        {
            Debug.LogError(name + " can't locate SpriteRenderer in children of the parent object.");
        }

        // Check and log missing reference for the projectilePrefab.
        if (projectilePrefab == null)
        {
            Debug.LogError(name + " is missing a reference to projectilePrefab. Please set one in the inspector.");
        }
        // Check and log missing reference for the crosshairPrefab if showCrosshair is enabled.
        if (showCrosshair == true && crosshairPrefab == null)
        {
            Debug.LogError(name + " is missing a reference to crosshairPrefab. Please set one in the inspector or disable showCrosshair in the inspector");
        }
        // Check and log missing reference for the explosionPrefab.
        if (explosionPrefab == null)
        {
            Debug.Log(name + " is missing a reference to explosionPrefab. Please set one in the inspector for this effect.");
        }

        // Check and log missing references for audio sources.
        if (readySound == null)
        {
            Debug.Log(name + " is missing a reference to readySound. Please set one in the inspector for this effect.");
        }
        if (windupSound == null)
        {
            Debug.Log(name + " is missing a reference to windupSound. Please set one in the inspector for this effect.");
        }
        if (firingSound == null)
        {
            Debug.Log(name + " is missing a reference to firingSound. Please set one in the inspector for this effect.");
        }
        if (winddownSound == null)
        {
            Debug.Log(name + " is missing a reference to winddownSound. Please set one in the inspector for this effect.");
        }
        if (cooldownSound == null)
        {
            Debug.Log(name + " is missing a reference to cooldownSound. Please set one in the inspector for this effect.");
        }
        if (launchSound == null)
        {
            Debug.Log(name + " is missing a reference to launchSound. Please set one in the inspector for this effect.");
        }
        if (explosionSound == null)
        {
            Debug.Log(name + " is missing a reference to explosionSound. Please set one in the inspector for this effect.");
        }
    }

    // Draw gizmos for debugging purposes.
    private void OnDrawGizmos()
    {
        // Check if DEBUG_MODE is enabled.
        if (DEBUG_MODE)
        {
            if (projectiles != null)
            {
                // Iterate through each projectile in the list.
                for (int i = 0; i < projectiles.Count; i++)
                {
                    // Check if the projectile exists.
                    if (projectiles[i] != null)
                    {
                        // Set Gizmos color to red for projectile rays.
                        Gizmos.color = Color.red;
                        // Draw a ray from the projectile towards its up direction, representing the destroyRange.
                        Gizmos.DrawRay(projectiles[i].transform.position, projectiles[i].transform.up * destroyRange);

                        // Check the lock-on setting for additional visualization.
                        if (lockOnSetting == lockOn.OverlapCircleLockOn)
                        {
                            // If using Overlap Circle Lock-On, set Gizmos color to blue.
                            Gizmos.color = Color.blue;
                            // Draw a wire sphere around the projectile, representing the detectionRange.
                            Gizmos.DrawWireSphere(projectiles[i].transform.position, detectionRange);
                        }
                        else if (lockOnSetting == lockOn.RayCastLockOn)
                        {
                            // If using RayCast Lock-On, set Gizmos color to yellow.
                            Gizmos.color = Color.yellow;
                            // Draw a ray from the projectile towards its up direction, representing the detectionRange.
                            Gizmos.DrawRay(projectiles[i].transform.position, projectiles[i].transform.up * detectionRange);
                        }
                    }
                }
            }            

            // Draw a wire sphere representing the target and crosshair position.
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + (Vector3)targetDistance, 0.3f);
        }
    }
}