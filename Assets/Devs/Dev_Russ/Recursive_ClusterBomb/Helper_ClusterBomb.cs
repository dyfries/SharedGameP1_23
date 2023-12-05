using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper_ClusterBomb : Ability_Simple
{
    public Recursive_ClusterBomb bombToSpawn;
    protected override void StartFiring()
    {
        base.StartFiring();

        Recursive_ClusterBomb rb = Instantiate<Recursive_ClusterBomb>(bombToSpawn, transform.position + (Vector3)bombToSpawn.spawnOffset, Quaternion.identity);
        rb.ability = this;
        Rigidbody2D rigid = rb.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
    }
}
