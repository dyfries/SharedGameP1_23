using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ability_StarWeaving : Ability_Simple
{
    [Header("Dash Settings")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Vector2 _defaultDashDirection = Vector2.right;
    [SerializeField] private float _dashSpeed = 1f;
    [Header("Projectile Settings")]
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private float _projectileSpawnInterval = 0.2f;

    private float _projectileTimer = 0;

    protected override void StartFiring()
    {
        base.StartFiring();
        _projectileTimer = 0f;
        StartCoroutine(StartAbilityDash());
    }

    protected override void StartWinddown()
    {
        base.StartWinddown();
        // Just to be safe.
        StopAllCoroutines();
    }

    private IEnumerator StartAbilityDash()
    {
        // Cache dash direction.
        Vector2 direction = _rb.velocity.normalized;
        // Get the ability owner's rotation.
        Quaternion projectileRotation = _rb.transform.rotation;

        if (direction == Vector2.zero) direction = _defaultDashDirection;

        while (stageOfAbility == StageOfAbility.firing)
        {
            // Spawn projectiles at intervals.
            if (_projectileTimer <= 0f)
            {
                _projectileTimer = _projectileSpawnInterval;
                // Spawn projectiles.
                GameObject projectile = Instantiate(_projectile, _projectileSpawnPoint.position, projectileRotation);
            }
            else
            {
                _projectileTimer -= Time.deltaTime;
            }

            // Do dash.
            _rb.velocity = direction * _dashSpeed;

            yield return null;
        }
        yield return null;
    }
}
