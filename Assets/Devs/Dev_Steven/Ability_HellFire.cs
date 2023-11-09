using UnityEngine;

public class Ability_HellFire : Ability_Simple
{
    private SpriteRenderer playerSpriteRenderer;

    public GameObject projectilePrefab;
    public GameObject explosionPrefab;

    //public float growSpeed = 4f;
    //public float maxSize = 1f;
    //public float destroyAbilityIn = 2f;

    public int projectileCount;
    public float projectileSpeed;
    private float angleStep;

    private GameObject[] projectiles;

    private void Awake()
    {
        playerSpriteRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>();
        if (playerSpriteRenderer == null)
        {
            Debug.LogError(gameObject.name + " can't locate SpriteRenderer in children of parent object.");
        }
    }

    void Start()
    {
        angleStep = 360f / projectileCount;
    }

    protected override void Update()
    {
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

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = i * angleStep;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

            GameObject projectile;
            //projectile = Instantiate(projectilePrefab, transform.position, Random.rotation, gameObject.transform);
            projectile = Instantiate(projectilePrefab, transform.position, rotation, gameObject.transform);
            projectile.GetComponent<Rigidbody2D>().AddForce(projectile.transform.up * projectileSpeed, ForceMode2D.Impulse);

            Destroy(projectile, 3f);
        }

        base.StartWindup();
    }

    protected override void StartFiring()
    {
        playerSpriteRenderer.color = Color.red;

        for (int i = 0; i < projectileCount; i++)
        {
            //projectiles[i].transform.Translate(transform.up);
        }

        base.StartFiring();
    }

    protected override void StartWinddown()
    {
        playerSpriteRenderer.color = Color.grey;

        for (int i = 0; i < projectileCount; i++)
        {
            //Destroy(projectiles[i], 2f);
            //GameObject explosion;
            //explosion = Instantiate(explosionPrefab, projectiles[i].gameObject.transform.position, projectiles[i].gameObject.transform.rotation);
            //Destroy(explosion, 1.5f);
        }

        base.StartWinddown();
    }

    protected override void StartCooldown()
    {
        playerSpriteRenderer.color = Color.magenta;

        base.StartCooldown();
    }
}