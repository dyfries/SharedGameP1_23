using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_ShotGunBlast : Ability_Simple
{

    [Header("Now in Ability ShotGunBlast Subclass")]
    public GameObject projectile; //rigidbody2d?
    public Transform startPoint;

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
        /*GameObject newProjectile = */
        Instantiate(projectile, startPoint/*,Quaternion.AngleAxis()    Quaternion.Angle(transform.rotation, */);
        Instantiate(projectile, startPoint);
        Instantiate(projectile, startPoint);

    }

}
