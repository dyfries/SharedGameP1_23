using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Block : Ability_Simple
{
    [Header("Ability Block Subclass")]
    public GameObject Block;
    public GameObject BrokenBlock;
    protected override void StartFiring()
    {
        base.StartFiring();
        Block.GetComponent<CircleCollider2D>().enabled = true;
        BrokenBlock.SetActive(false);
    }

    protected override void StartWinddown()
    {
        base.StartWinddown();
        Block.GetComponent<CircleCollider2D>().enabled = false;
        BrokenBlock.SetActive(true);
        Block.SetActive(false);
    }
}
