using System.Collections;
using System.Collections.Generic;
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

    protected override void StartWindup()
    {
        base.StartWindup();

        spriteRenderer.color = invinsibilityColor;
    }

    protected override void StartFiring()
    {
        base.StartFiring();

        // Stop collisions by turning off collider
        // Stop collisions with enemies only
        collide.excludeLayers = npcLayer;
        //collide.enabled = false;
    }

    protected override void StartCooldown()
    {
        base.StartCooldown();

        // Return to colliding
        //collide.enabled = true;
        collide.excludeLayers = nothingLayer;

        // Stop invinsibility flashing
        spriteRenderer.color = defaultColor;
    }
}
