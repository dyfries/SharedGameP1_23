using System.Collections.Generic;
using UnityEngine;

// The HellFire ability inherits from the Ability_Simple class
public class Ability_HellFire : Ability_Simple
{
    [Header("Ability Settings")]
    [Range(1, 100)]
    [SerializeField] private int projectileCount = 9;                   // Number of projectiles.
    [Range(0.1f, 20f)]
    [SerializeField] private float launchSpeed = 2f;                    // Speed of projectile launch.    
    [Range(0.1f, 20f)]
    [SerializeField] private float targetSpeed = 4f;                    // Speed of projectile targeting movement.
    [SerializeField] private Vector2 targetDistance;                    // Position of the crosshair from the player.
    private Vector3 crosshairPosition;                                  // Current position of the crosshair.
    [Range(0f, 1000f)]
    [SerializeField] private float rotationSpeed = 150f;                // Base speed of projectile rotation.
    [Range(0f, 1000f)]
    [SerializeField] private float lockOnRotationSpeed = 200f;          // Speed of lock-on rotation.
    [Range(0f, 200f)]
    [SerializeField] private float firingDrag = 20f;                    // Drag applied to the player during firing.
    private float startingDrag;                                         // Initial drag value of the player.
    [Range(0.1f, 20f)]
    [SerializeField] private float growDuringFiringRate = 2.0f;         // Rate at which projectiles grow during firing.
    [Range(0.1f, 20f)]
    [SerializeField] private float shrinkDuringWinddownRate = 2.0f;     // Rate at which projectiles shrink during winddown.
    [SerializeField] private Vector2 startScale = new Vector2(0.5f, 1f);// Initial scale of the projectiles.
    [SerializeField] private Vector2 middleScale = new Vector2(6f, 12f);// Intermediate scale of the projectiles during firing.
    [SerializeField] private Vector2 endScale = new Vector2(0.5f, 1f);  // Final scale of the projectiles during winddown.
    [Range(0.01f, 5f)]
    [SerializeField] private float detectionRange = 2f;                 // Range for NPC lock-on detection.
    [Range(0.01f, 5f)]
    [SerializeField] private float destroyRange = 0.2f;                 // Range for NPC hit detection.
    [Range(0.1f, 5f)]
    [SerializeField] private float explosionDestroyTime = 1.5f;         // Time for the explosion effect to destroy itself.

    [Header("Ability Toggles")]
    [SerializeField] private bool randomSpawnAngle = false;             // Toggle for random projectile spawn angles.
    private float angleStep;                                            // Angle step between projectiles.
    [SerializeField] private bool firingPause = true;                   // Toggle to apply drag to the player during firing.
    [SerializeField] private bool growDuringFiring = false;             // Toggle to enable projectile growth during firing.
    [SerializeField] private bool shrinkDuringWinddown = false;         // Toggle to enable projectile shrinkage during winddown.
    [SerializeField] private bool showCrosshair = true;                 // Toggle to show the crosshair.
    [SerializeField] private bool destroyProjectileOnCollision = true;  // Toggle for destroying projectiles on NPC collision.
    [SerializeField] private bool destroyNPCOnCollision = true;         // Toggle for destroying NPCs on collision.
    [SerializeField] private LockOn lockOnSetting;                      // Enum for lock-on settings.
    [SerializeField] private enum LockOn
    {
        DisableLockOn, RayCastLockOn, OverlapCircleLockOn, 
    }

    [Header("References & Layers")]
    [SerializeField] private GameObject projectilePrefab;               // Prefab for the projectile.
    [SerializeField] private GameObject crosshairPrefab;                // Prefab for the crosshair.
    [SerializeField] private GameObject explosionPrefab;                // Prefab for the explosion effect.
    [SerializeField] private GameObject missileDock;                    // Gameobject to visualize ready after cooldown
    [SerializeField] private GameObject projectileHolder;               // GameObject to organize spawned projectiles
    [SerializeField] private LayerMask NPCLayers;                       // Layers considered for NPC interactions.

    private SpriteRenderer playerSpriteRenderer;                        // Reference to the player's sprite renderer.
    private Rigidbody2D playerRidigbody;                                // Reference to the player's rigidbody2D.
    private List<GameObject> projectiles;                               // List to store instantiated projectiles.
    private List<Vector3> targetPositions;                              // List to store target positions for each projectile.

    [Header("Audio Sources")]
    [SerializeField] private AudioSource readySound;                    // Sound played when the ability is ready.
    [SerializeField] private AudioSource windupSound;                   // Sound played at windup state.
    [SerializeField] private AudioSource firingSound;                   // Sound played at firing state.
    [SerializeField] private AudioSource winddownSound;                 // Sound played at winddown state.
    [SerializeField] private AudioSource cooldownSound;                 // Sound played at cooldown state.

    [SerializeField] private AudioSource launchSound;                   // Sound played when projectiles are launched.
    [SerializeField] private AudioSource explosionSound;                // Sound played when projectile destroyed.


    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        // Locate SpriteRenderer in children of the parent object.
        playerSpriteRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>();

        // Get the Rigidbody2D component from the parent object.
        playerRidigbody = transform.parent.GetComponent<Rigidbody2D>();

        // Set the initial drag value of the player.
        if (firingPause && playerRidigbody != null)
        {
            startingDrag = playerRidigbody.drag;
        }

        // Initialize lists for projectiles and target positions.
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
        // Update projectiles movement, rotation, scale, and target based on the current stage of the ability.
        ProjectilesBehavior();

        // Adjust the scale of the missileDock based on the current stage of the ability.
        MissleDockReload();
        
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
                // Instantiate projectile, set its name & starting scale, and add it to the list.
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
                projectile.transform.localScale = new Vector3(startScale.x, startScale.y, 1f);                

                projectiles.Add(projectile);
            }

            // Play launch sound if available.
            if (launchSound != null)
            {
                launchSound.Play();
            }
        }

        // Check if firingPause is enabled and playerRidigbody is not null.
        if (firingPause && playerRidigbody != null)
        {
            // Set drag of the player's Rigidbody2D back to the firing drag
            playerRidigbody.drag = firingDrag;
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

                // Get the ParticleSystem component of the projectile.
                ParticleSystem particles = projectiles[i].GetComponentInChildren<ParticleSystem>();

                // Play particle effects if available
                if (particles != null)
                {
                    particles.Play();
                }
            }
        }

        // Check if firingPause is enabled and playerRidigbody is not null.
        if (firingPause && playerRidigbody != null)
        {
            // Set drag of the player's Rigidbody2D back to the initial starting drag.
            playerRidigbody.drag = startingDrag;
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

    // Update projectiles movement, rotation, scale, and target based on the current stage of the ability.
    private void ProjectilesBehavior()
    {
        // Check if projectiles exist.
        if (projectiles.Count > 0)
        {
            // Iterate through each projectile.
            for (int i = 0; i < projectiles.Count; i++)
            {
                // Check if the projectile and target position are not null.
                if (projectiles[i] != null && targetPositions[i] != null)
                {
                    // Check if the ability is in the "firing" or "winddown" state.
                    if (stageOfAbility == StageOfAbility.firing || stageOfAbility == StageOfAbility.winddown)
                    {
                        // Check if the projectile is within target range.
                        CrosshairRangeDetection(i);
                        // Perform NPC lock-on detection if enabled.
                        if (lockOnSetting != LockOn.DisableLockOn)
                        {
                            NPCLockOnDetection(i);
                        }
                        // Perform NPC hit detection.
                        NPCHitDetection(i);
                    }

                    // Check if the ability is in the "firing" state.
                    if (stageOfAbility == StageOfAbility.firing)
                    {
                        // Move the projectile forward at launchSpeed.
                        projectiles[i].transform.position = projectiles[i].transform.position + launchSpeed * Time.deltaTime * projectiles[i].transform.up;

                        // Check if growDuringFiring is enabled.
                        if (growDuringFiring)
                        {
                            // Check if the current scale of the projectile is less than the middle scale.
                            if (projectiles[i].transform.localScale.x < middleScale.x)
                            {
                                // Increase the projectile's scale based on the growth rate and time.
                                projectiles[i].transform.localScale += new Vector3(growDuringFiringRate * Time.deltaTime, growDuringFiringRate * Time.deltaTime, 0f);
                            }
                        }
                    }

                    // Check if the ability is in the "winddown" state.
                    if (stageOfAbility == StageOfAbility.winddown)
                    {
                        // Calculate rotation angle towards the target position.
                        Vector3 rotateTowards = (targetPositions[i] - projectiles[i].transform.position).normalized;
                        float smoothRotateAngle = Vector2.SignedAngle(projectiles[i].transform.up, rotateTowards);

                        // Smoothly rotate the projectile based on the calculated angle.
                        if (smoothRotateAngle > 0.1f)
                        {
                            // Check if the target position is not the crosshair position.
                            if (targetPositions[i] != crosshairPosition)
                            {
                                // Rotate the projectile towards the target with lock-on rotation speed.
                                projectiles[i].transform.Rotate(0f, 0f, lockOnRotationSpeed * Time.deltaTime);
                            }
                            else
                            {
                                // Rotate the projectile towards the target with regular rotation speed.
                                projectiles[i].transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
                            }
                        }
                        else if (smoothRotateAngle < -0.1f)
                        {
                            // Check if the target position is not the crosshair position.
                            if (targetPositions[i] != crosshairPosition)
                            {
                                // Rotate the projectile towards the target with negative lock-on rotation speed.
                                projectiles[i].transform.Rotate(0f, 0f, -lockOnRotationSpeed * Time.deltaTime);
                            }
                            else
                            {
                                // Rotate the projectile towards the target with negative regular rotation speed.
                                projectiles[i].transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
                            }
                        }

                        // Move the projectile forward at targetSpeed.
                        projectiles[i].transform.position = projectiles[i].transform.position + targetSpeed * Time.deltaTime * projectiles[i].transform.up;

                        // Check if shrinkDuringWinddown is enabled.
                        if (shrinkDuringWinddown)
                        {
                            // Check if the projectile's current scale is larger than the specified final scale.
                            if (projectiles[i].transform.localScale.x > endScale.x)
                            {
                                // Shrink the projectile's scale based on the specified rate and deltaTime.
                                projectiles[i].transform.localScale -= new Vector3(shrinkDuringWinddownRate * Time.deltaTime, shrinkDuringWinddownRate * Time.deltaTime, 0f);
                            }
                        }
                    }
                }
            }
        }
    }

    // Adjust the scale of the missileDock based on the current stage of the ability.
    private void MissleDockReload()
    {
        // Check if the missileDock reference is not null.            
        if (missileDock != null)
        {
            // Check if the ability is in the "ready" state.
            if (stageOfAbility == StageOfAbility.ready)
            {
                // Increase the missileDock scale gradually if it's below the threshold.
                if (missileDock.transform.localScale.x <= 1)
                {
                    missileDock.transform.localScale += new Vector3(2f, 0f, 0f) * Time.deltaTime;
                }   
            }
            else
            {
                // Decrease the missileDock scale gradually if it's above zero.
                if (missileDock.transform.localScale.x >= 0f)
                {
                    missileDock.transform.localScale -= new Vector3(4f, 0f, 0f) * Time.deltaTime;
                }           
            }
        }
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
        if (lockOnSetting == LockOn.RayCastLockOn)
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
        else if (lockOnSetting == LockOn.OverlapCircleLockOn)
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

    // Method to destroy a projectile, play explosion sound, and create explosion effect.
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
        // Check and log missing reference for the players SpriteRenderer.
        if (playerSpriteRenderer == null)
        {
            Debug.LogError(name + " can't locate a SpriteRenderer in children of the parent object.");
        }
        // Check and log missing reference for the players Rigidbody2D if firingPause is enabled.
        if (firingPause == true && playerRidigbody == null)
        {
            Debug.LogError(name + " can't locate a Rigidbody2D on the parent object for the firingPause effect.");
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

        // Check and log missing reference for the projectileHolder.
        if (projectileHolder == null)
        {
            Debug.Log(name + " is missing a reference to a projectileHolder. Please set one in the inspector for this effect.");
        }
        // Check and log missing reference for the missileDock.
        if (missileDock == null)
        {
            Debug.Log(name + " is missing a reference to a missileDock. Please set one in the inspector for this effect.");
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
                        if (lockOnSetting == LockOn.OverlapCircleLockOn)
                        {
                            // If using Overlap Circle Lock-On, set Gizmos color to blue.
                            Gizmos.color = Color.blue;
                            // Draw a wire sphere around the projectile, representing the detectionRange.
                            Gizmos.DrawWireSphere(projectiles[i].transform.position, detectionRange);
                        }
                        else if (lockOnSetting == LockOn.RayCastLockOn)
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