using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Recursion : Ability_Simple
{

   public RecursionBomb bombToSpawn;
   protected override void StartFiring() {
        base.StartFiring();

        RecursionBomb rb = Instantiate<RecursionBomb>(bombToSpawn, transform.position + (Vector3)bombToSpawn.spawnOffsetLeft, Quaternion.identity);
        rb.ability = this;
        RecursionBomb rb1 = Instantiate<RecursionBomb>(bombToSpawn, transform.position + (Vector3)bombToSpawn.spawnOffsetRight, Quaternion.identity);
        rb1.ability = this;
    }
}
