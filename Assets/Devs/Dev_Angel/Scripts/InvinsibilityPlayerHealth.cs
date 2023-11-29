using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityPlayerHealth : MonoBehaviour
{
    [Header("Health")]
    private int currentHealth = 100;
    public int CurrentHealth => currentHealth;

    [Header("Animations")]
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerArtController playerArtController;

    [Header("** Debug Console Logs **")]
    [SerializeField] private bool debug_Health;
	[SerializeField] private bool debug_Damaged;

    public void Damage(int damage)
    {
		// Lower health
        currentHealth -= damage;

		// If if player is dead
		CheckIfDied();

		// Debugs if debug_Damaged is true
		if (debug_Damaged) { Debug.Log(gameObject.name + " was damaged!");  }
    }

	// Check if players health is too low and run animation and destroy object
	private void CheckIfDied() // Should be put in a player art script, but dont want to modify main script
	{
		// Check health only when animation references aren't null
		if (anim != null && playerArtController != null)
		{
			// If health goes under 0, start animation and destroy self after animation time
			// And turn off art controller script to be able to use animator
			if (currentHealth <= 0)
			{
				playerArtController.enabled = false;
				anim.SetTrigger("Explode");
				Destroy(gameObject, 1.083f);
			}
		}
	}

	private void Update()
	{
		// Debugs if debug_Health is true
		if (debug_Health) { Debug.Log(gameObject.name + " Current Health: " + currentHealth); }
	}
}
