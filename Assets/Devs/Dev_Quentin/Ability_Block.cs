using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Block : Ability_Simple
{
    [Header("Ability Block Subclass")]
    public GameObject ArtBlock;
    //public GameObject BrokenBlock;

    [Header(" --- Animator ---  ")]
    public Animator anim;
    private string anim_windupString = "Shield_Windup";
    private string anim_firingString = "Shield_On";
    private string anim_winddownString = "Shield_Winddown";

    public AudioSource staticSound;
    public AudioSource boomSound;

    // For refrence, the way the shield should work is that the shield animtion will play when you press a button.
    // Then when it hits "StartWinddown" it will play the recharge animation for a set amount of time.
    // After that set amount of time has passed there would be a visual effect that would play, showing that you can use the shield again.

    protected void Start()
    {
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
            if (anim == null)
            {
                Debug.LogWarning("Animator cannot be found by Block ability");
            }
        }
    }
    protected override void StartWindup()
    {
        base.StartWindup();

        staticSound.Play();
        anim.SetBool(anim_windupString, true);

    }

    protected override void StartFiring()
    {
        base.StartFiring();

        boomSound.Play();
        anim.SetBool(anim_windupString, false);
        anim.SetBool(anim_firingString, true);

    }

    protected override void StartWinddown()
    {
        base.StartWinddown();

        staticSound.Stop();
        anim.SetBool(anim_firingString, false);
        anim.SetBool(anim_winddownString, true);
    }

    protected override void StartCooldown()
    {
        base.StartCooldown();

        anim.SetBool(anim_winddownString, false);

    }
}
