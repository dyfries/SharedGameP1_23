using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Block : Ability_Simple
{
    // Headers are used mainly for organizing variables in Unity
    [Header("Ability Block Subclass")]
    public GameObject ArtBlock;

    // Animation variables
    [Header(" --- Animator ---  ")]
    public Animator anim;
    private string anim_windupString = "Shield_Windup";
    private string anim_firingString = "Shield_On";
    private string anim_winddownString = "Shield_Winddown";

    // Add audio sources for static sound effect and a thunder sound effect
    // public variables for adding audio
    public AudioSource staticSound;
    public AudioSource boomSound;

    // For refrence, the way the shield basically works like a burst/parry option.
    // The animation plays quickly. 

    protected void Start()
    {
        // Finds Animator component and checks if it's working if not, it gives a debug.warning 
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
            if (anim == null)
            {
                Debug.LogWarning("Animator cannot be found by Block ability");
            }
        }
    }
    
    // Calls animation states from parent class "Ability_Simple"
    // Bools are used to set animation states to true or false
    protected override void StartWindup()
    {
        base.StartWindup();

        // Plays static sound effect from audio source when animation hits this state
        staticSound.Play();
        anim.SetBool(anim_windupString, true);

    }

    protected override void StartFiring()
    {
        base.StartFiring();

        // Plays boom(thunder) sound effect from audio source when animation hits this state
        boomSound.Play();
        anim.SetBool(anim_windupString, false);
        anim.SetBool(anim_firingString, true);

    }

    protected override void StartWinddown()
    {
        base.StartWinddown();

        anim.SetBool(anim_firingString, false);
        anim.SetBool(anim_winddownString, true);
    }

    protected override void StartCooldown()
    {
        base.StartCooldown();

        anim.SetBool(anim_winddownString, false);

    }
}
