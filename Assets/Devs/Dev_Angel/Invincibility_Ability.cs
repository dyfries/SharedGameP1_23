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

	[Header("Audio")]
	[SerializeField] private AudioSource thisAudioOutput;
	[SerializeField] private AudioClip invincibilitySound;
	[SerializeField] private AudioSource minionLeftAudioOutput;
	[SerializeField] private AudioSource minionRightAudioOutput;
	[SerializeField] private AudioClip minionState;

    private void Start()
    {
		// Setting this audio source
        thisAudioOutput = gameObject.GetComponent<AudioSource>();
    }

    protected override void StartWindup()
	{
		base.StartWindup();

		if(thisAudioOutput != null)
		{
            thisAudioOutput.loop = false; // Turn off looping from spaceship moving noise
            thisAudioOutput.clip = invincibilitySound;
            thisAudioOutput.Play();
        }
		if(spriteRenderer != null)
		{
            spriteRenderer.color = invinsibilityColor;
        }
    }

	protected override void StartFiring()
	{
		base.StartFiring();

		// Stop collisions by turning off collider
		// Stop collisions with enemies only
		collide.excludeLayers = npcLayer;
		//collide.enabled = false;

		SpawnMinions();
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

		// Playing death sound
        //	Left
        minionLeftAudioOutput.loop = false; // Turn off looping from buzzing noise
        minionLeftAudioOutput.clip = minionState;
        minionLeftAudioOutput.Play();

        //	Right
        minionLeftAudioOutput.loop = false; // Turn off looping from buzzing noise
        minionLeftAudioOutput.clip = minionState;
        minionLeftAudioOutput.Play();

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

		// Setting minion audioOutputs
		minionLeftAudioOutput = leftMinion.GetComponent<AudioSource>();
		minionRightAudioOutput = rightMinion.GetComponent<AudioSource>();

		// Play spawning sound
		//	Left
		minionLeftAudioOutput.loop = false; // Turn off looping from buzzing noise
		minionLeftAudioOutput.clip = minionState;
		minionLeftAudioOutput.Play();

		//	Right
		minionLeftAudioOutput.loop = false; // Turn off looping from buzzing noise
		minionLeftAudioOutput.clip = minionState;
		minionLeftAudioOutput.Play();

	}
}
