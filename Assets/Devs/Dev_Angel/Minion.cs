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
	[SerializeField] private float moveForce = 10;
	private float movingTimer;

	[Header("Find NPC")]
	private BaseNPC npc;
	private float findEnemyRadius = 15;

	[Header("Self Destruct")]
	private float destructTimer;
	private float destructTimeAmount = 10;

	private void Start()
	{
		rigid = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		movingTimer += Time.deltaTime;
		destructTimer += Time.deltaTime;
		currentPosition = transform.position;


		// Find the closest enemy within range
		FindClosestEnemy();

		// Move towards the closest NPC enemy
		MoveToClosestNPC();

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

		Debug.Log(moveTowards.normalized);
        Debug.DrawRay(currentPosition, closestEnemyPosition);
    }

	private void HitNPC()
    {

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
