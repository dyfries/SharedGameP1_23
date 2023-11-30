using UnityEngine;

public class SimpleInvincibilityHealthBar : MonoBehaviour
{
	[SerializeField] private InvincibilityPlayerHealth health;

	[Header("Health Bars")]
	[SerializeField] private SpriteRenderer bar1;
	[SerializeField] private SpriteRenderer bar2;
	[SerializeField] private SpriteRenderer bar3;

	private void Update()
	{
		// If nothing is null change bar colors depending on current health
		if(health != null && bar1 != null && bar2 != null && bar3 != null)
		{
			if(health.CurrentHealth <= 0)
			{
				bar3.color = Color.red;
			}
			else if(health.CurrentHealth <= 30)
			{
				bar2.color = Color.red;
			}
			else if(health.CurrentHealth <= 60)
			{
				bar1.color = Color.red;
			}
		}
	}
}
