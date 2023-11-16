using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Minion : MonoBehaviour
{ 
	[Header("Move")]
	private Rigidbody2D rigid;
	private Vector3 currentPosition;
	private Vector3 closestEnemyPosition;
	private Vector3 moveTowards;
	[SerializeField] private float moveForce = 25;

	[Header("Find NPC")]
	private BaseNPC npc;
	private float findEnemyRadius = 15;

	[Header("Self Destruct")]
	[SerializeField] private float destructTimer;
	[SerializeField] private float destructTimeAmount = 10;

	[Header("Hit NPC")]
	private float distanceBetween;
	private float hitDistance = 0.5f;

	private void Start()
	{
		rigid = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		// Updating destruct timer and current position
		destructTimer += Time.deltaTime;
		currentPosition = transform.position;

		// Find the closest enemy within range
		FindClosestEnemy();

		// Move towards the closest NPC enemy
		MoveToClosestNPC();

		// When distance is small damage NPC
		HitNPC();

		// Self destruct after time
		SelfDestruct();
    }

	// If npc is null find a new closest enemy within range
	private void FindClosestEnemy()
	{
		RaycastHit2D hit = Physics2D.CircleCast(currentPosition, findEnemyRadius, Vector2.zero);
		
		// Only set the first time or the closest npc (when the npc is null)
		if(npc == null)
		{
			npc = hit.transform.GetComponent<BaseNPC>();
		}
	}

	private void MoveToClosestNPC()
	{
		// If npc is not null set the position
		if (npc != null) { closestEnemyPosition = npc.transform.position; }

		// Find the direction the minion should move
		moveTowards = closestEnemyPosition - currentPosition;

		// Add a move force in that direction
		rigid.AddForce(moveTowards.normalized * moveForce);
    }

	// When the distance between NPC and minion is small enough, damage NPC or kill NPC
	private void HitNPC()
    {
		// Finding the distance between npc vector and currnet position vector
		distanceBetween = Mathf.Sqrt(Mathf.Pow(closestEnemyPosition.x - currentPosition.x, 2) + Mathf.Pow(closestEnemyPosition.y - currentPosition.y, 2));

		if (distanceBetween < hitDistance)
        {
			// Destroy npc --- change to damage npc
			Destroy(npc.gameObject);
        }
    }

	// After destructTimeAmount seconds self destruct
	private void SelfDestruct()
    {
		if(destructTimer >= destructTimeAmount)
        {
			Destroy(gameObject);
        }
    }
}
