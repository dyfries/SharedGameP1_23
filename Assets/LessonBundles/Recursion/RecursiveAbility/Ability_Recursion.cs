using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Recursion : Ability_Simple
{

    public RecursionBomb bombToSpawn;
    public Vector2 initialOffset = Vector2.up;
    protected override void StartFiring() {
        base.StartFiring();

        RecursionBomb rb = Instantiate<RecursionBomb>(bombToSpawn, transform.position + (Vector3)initialOffset, Quaternion.identity);
        rb.ability = this;
    }
}
