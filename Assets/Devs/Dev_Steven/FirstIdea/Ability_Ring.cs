using UnityEngine;

public class Ability_Ring : Ability_Simple
{
    public GameObject ringAbilityPrefab;
    public GameObject explosionPrefab;
    private GameObject ability;
    private SpriteRenderer playerSpriteRenderer;

    public float rotationSpeed = 5f;
    public float growSpeed = 4f;
    public float maxSize = 1f;
    public float destroyAbilityIn = 2f;

    private void Awake()
    {
        playerSpriteRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>();
        if (playerSpriteRenderer == null )
        {
            Debug.LogError(gameObject.name + " can't locate SpriteRenderer in children of parent object.");
        }        
    }

    void Start()
    {

    }

    protected override void Update()
    {
        if (ability != null)
        {
            ability.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

            if (ability.transform.localScale.x < maxSize)
            {
                ability.transform.localScale += new Vector3(0.1f, 0.1f, 0) * Time.deltaTime * growSpeed;
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

        base.StartWindup();
    }

    protected override void StartFiring()
    {
        playerSpriteRenderer.color = Color.red;

        ability = Instantiate(ringAbilityPrefab, transform.parent.transform.localPosition, transform.rotation, transform);        

        base.StartFiring();
    }

    protected override void StartWinddown()
    {
        playerSpriteRenderer.color = Color.grey;

        for (int i = 0; i < ability.transform.childCount; i++)
        {
            Destroy(ability.transform.GetChild(i).gameObject);
            
            GameObject explosion;
            explosion = Instantiate(explosionPrefab, ability.transform.GetChild(i).gameObject.transform.position, ability.transform.GetChild(i).gameObject.transform.rotation);
            Destroy(explosion, 1.5f);
            
        }
        
        base.StartWinddown();
    }

    protected override void StartCooldown()
    {
        playerSpriteRenderer.color = Color.magenta;

        Destroy(ability);

        base.StartCooldown();
    }
}