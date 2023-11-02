using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_ShotGunBlast : Ability_Simple
{

    [Header("Now in Ability ShotGunBlast Subclass")]
    public GameObject projectile;
    public Transform playerPos;

    protected void Start()
    {
        if (projectile == null)
        {
            Debug.LogWarning("no projectile");
            enabled = false;
        }
    }

    protected override void StartFiring()
    {

        base.StartFiring();

        // Add projectiles

        Instantiate(projectile, playerPos, /*Quaternion.Angle(*/);
        Instantiate(projectile, playerPos);
        Instantiate(projectile, playerPos);

    }

}
