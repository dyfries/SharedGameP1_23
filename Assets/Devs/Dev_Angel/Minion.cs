using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Minion : MonoBehaviour
{ 
	private Vector3 currentPosition;
	private Vector3 closestEnemyPosition;
	private Vector3 moveTowards;
	[SerializeField] private float findEnemyRadius;
	private Rigidbody2D rigidbody;

	[SerializeField]private BaseNPC npc;

	[SerializeField]private float moveForce = 10;
	private float movingTimer;

	private void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}
	private void Update()
	{
		movingTimer += Time.deltaTime;
		currentPosition = transform.position;

		FindClosestEnemy();
		MoveToClosestNPC();

    }

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
		rigidbody.AddForce(moveTowards.normalized * moveForce * Time.deltaTime);

		Debug.Log(moveTowards.normalized);
        Debug.DrawRay(currentPosition, closestEnemyPosition);
    }
}
