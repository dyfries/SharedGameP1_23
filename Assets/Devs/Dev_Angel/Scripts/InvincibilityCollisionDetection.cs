using UnityEngine;
using UnityEngine.Events;

public class InvincibilityCollisionDetection : MonoBehaviour
{
	[SerializeField] private UnityEvent collisionEnter;
	[SerializeField] private UnityEvent collisionExit;

	[Header("** Debug Console Logs **")]
	[SerializeField] private bool debug_Collisions;

	// Invokes a set event on collision enter
	private void OnCollisionEnter2D(Collision2D collision)
	{
		collisionEnter.Invoke();

		// Debugs if debug_collisions is true
		if (debug_Collisions) { Debug.Log(gameObject.name + " Collision Enter: " + collision.gameObject.name); }
	}

	// Invokes a set event on collision exit
	private void OnCollisionExit2D(Collision2D collision)
	{
		collisionExit.Invoke();

		// Debugs if debug_collisions is true
		if (debug_Collisions) { Debug.Log(gameObject.name + " Collision Exit: " + collision.gameObject.name); }
	}
}
