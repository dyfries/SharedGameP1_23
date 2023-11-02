using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ability_StarWeaving : Ability_Simple
{
    [Header("Dash Settings")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Vector2 _defaultDashDirection = Vector2.right;
    [SerializeField] private float _dashVelocity = 1f;
    [Header("Projectile Settings")]
    [SerializeField] private Projectile_StarWeavingOrb _projectile;
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
        Vector2 direction = _rigidbody.velocity.normalized;
        // Get the ability owner's rotation.
        Vector2 abilityForwards = _rigidbody.transform.up;

        if (direction == Vector2.zero) direction = _defaultDashDirection;

        while (stageOfAbility == StageOfAbility.firing)
        {
            // Spawn projectiles at intervals.
            if (_projectileTimer <= 0f)
            {
                _projectileTimer = _projectileSpawnInterval;
                // Spawn projectiles.
                Projectile_StarWeavingOrb projectile = Instantiate(_projectile, _projectileSpawnPoint.position, Quaternion.identity);
                projectile.SetDirection(abilityForwards);
            }
            else
            {
                _projectileTimer -= Time.deltaTime;
            }

            // Do dash.
            _rigidbody.velocity = direction * _dashVelocity;

            yield return null;
        }
        yield return null;
    }
}
