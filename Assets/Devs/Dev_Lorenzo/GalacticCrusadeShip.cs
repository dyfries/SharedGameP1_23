using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalacticCrusadeShip : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private GameObject _projectile;
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _initialMovementDuration = 0.5f;
    [SerializeField] private float _firingDuration = 3f;
    [SerializeField] private float _exitLifetime = 10f;
    [SerializeField] private float _sineMoveStrength = 180f;
    [Header("Weapon Settings")]
    [SerializeField] private float _attackSpeed = 0.5f;        // In real seconds.

    private float _timer = 0f;
    private float _projectileTimer = 0f;

    private void Start()
    {
        StartCoroutine(StartAttack());
    }

    private void Move()
    {
        transform.Translate(Vector2.up * Time.deltaTime * _moveSpeed);
    }

    private void FireProjectile()
    {
        Instantiate(_projectile, _projectileSpawnPoint.position, Quaternion.identity);
    }

    private IEnumerator StartAttack()
    {
        // Move upwards at the start.
        while (_timer < _initialMovementDuration)
        {
            transform.Translate(Vector2.up * Time.deltaTime * _moveSpeed);
            _timer += Time.deltaTime;
            yield return null;
        }
        // Strafe left and right while firing projectiles.
        _timer = 0f;
        while (_timer < _firingDuration)
        {
            // Strafe.
            float sinValue = Mathf.Sin(_timer * _sineMoveStrength);
            transform.Translate(Vector2.right * sinValue * Time.deltaTime);

            // Shoot projectiles.
            if (_projectileTimer >= _attackSpeed)
            {
                FireProjectile();
                _projectileTimer = 0f;
            }
            else _projectileTimer += Time.deltaTime;

            _timer += Time.deltaTime;
            yield return null;
        }
        // Continue moving until despawning.
        _timer = 0f;
        while (_timer < _exitLifetime)
        {
            transform.Translate(Vector2.up * Time.deltaTime * _moveSpeed);
        }
    }
}
