using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Recursion : Ability_Simple
{

   public RecursionBomb bombToSpawn;
   protected override void StartFiring() {
        base.StartFiring();

        RecursionBomb rb = Instantiate<RecursionBomb>(bombToSpawn, transform.position + (Vector3)bombToSpawn.spawnOffset, Quaternion.identity);
        rb.ability = this;
    }
}
