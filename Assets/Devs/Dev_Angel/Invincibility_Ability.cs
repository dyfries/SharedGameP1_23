using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility_Ability : Ability_Simple
{
    [Header("Invinisbilty")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CircleCollider2D collide;
    private Color defaultColor = Color.white;
    private Color invinsibilityColor = Color.grey;

    protected override void StartWindup()
    {
        base.StartWindup();

        spriteRenderer.color = invinsibilityColor;
    }

    protected override void StartFiring()
    {
        base.StartFiring();

        // Stop collisions by turning off collider
        collide.enabled = false;
    }

    protected override void StartCooldown()
    {
        base.StartCooldown();

        // Return to colliding
        collide.enabled = true;

        // Stop invinsibility flashing
        spriteRenderer.color = defaultColor;
    }
}
