using UnityEngine;

public class Invincibility_Ability : Ability_Simple
{
    [Header("Invinisbilty")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CircleCollider2D collide;
    [SerializeField] private LayerMask nothingLayer;
    [SerializeField] private LayerMask npcLayer;
    private Color defaultColor = Color.white;
    private Color invinsibilityColor = Color.grey; // Change this to lerp colors

    [Header("Spawn Minions")]
    [SerializeField] private GameObject minionPrefab;
    private float spawnOffset = 1;
    private GameObject leftMinion;
    private GameObject rightMinion;
    private Animator leftAnim;
    private Animator rightAnim;

    protected override void StartWindup()
    {
        base.StartWindup();

        spriteRenderer.color = invinsibilityColor;
        Debug.Log("Starting");
    }

    protected override void StartFiring()
    {
        base.StartFiring();

        // Stop collisions by turning off collider
        // Stop collisions with enemies only
        collide.excludeLayers = npcLayer;
        //collide.enabled = false;

        SpawnMinions();

        Debug.Log("Firing");
    }

    protected override void StartCooldown()
    {
        base.StartCooldown();

        // Return to colliding
        //collide.enabled = true;
        collide.excludeLayers = nothingLayer;

        // Stop invinsibility flashing
        spriteRenderer.color = defaultColor;

        // Play minions exploding animation
        leftAnim.SetTrigger("Explode");
        rightAnim.SetTrigger("Explode");

        // Self destruct minions
        leftMinion.GetComponent<Minion>().SelfDestruct();
        rightMinion.GetComponent<Minion>().SelfDestruct();
    }

    private void SpawnMinions()
    {
        // Setting a new position to spawn minions in with offset
        Vector3 leftMinionPosition = new Vector3(transform.position.x - spawnOffset, transform.position.y, 0);
        Vector3 rightMinionPosition = new Vector3(transform.position.x + spawnOffset, transform.position.y, 0);

        // Spawning minions in and caching their gameObjects
        leftMinion = Instantiate(minionPrefab, leftMinionPosition, Quaternion.identity);
        rightMinion = Instantiate(minionPrefab, rightMinionPosition, Quaternion.identity);

        // Getting animators to set explosion sprite
        leftAnim = leftMinion.GetComponent<Animator>();
        rightAnim = rightMinion.GetComponent<Animator>();
    }
}
