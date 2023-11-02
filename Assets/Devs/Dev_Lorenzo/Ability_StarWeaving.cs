using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_StarWeaving : Ability_Simple
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [Header("Dash Settings")]
    [SerializeField] private Vector2 _defaultDashDirection = Vector2.right;
    [SerializeField] private float _dashSpeed = 1f;
    [Header("Projectile Settings")]
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _projectileSpawnInterval = 0.2f;

    private float _projectileTimer = 0;

    protected override void StartFiring()
    {
        base.StartFiring();
        _projectileTimer = 0f;
    }

    private IEnumerator StartSpawningProjectiles()
    {
        yield return null;
    }
}
