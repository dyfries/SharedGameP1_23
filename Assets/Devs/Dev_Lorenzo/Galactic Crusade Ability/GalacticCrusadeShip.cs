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
    [SerializeField] private float _strafeSpeed = 1f;
    [SerializeField] private float _exitSpeed = 1f;
    [SerializeField] private float _initialMovementDuration = 0.5f;
    [SerializeField] private float _firingDuration = 3f;
    [SerializeField] private float _exitLifetime = 10f;
    [Header("Strafe Movement Settings")]
    [SerializeField] private float _sineMoveStrength = 180f;
    [Header("Weapon Settings")]
    [SerializeField, Min(0.01f)] private float _attackSpeed = 0.5f;        // In real seconds.
    [Header("Debug mode helps visualize strafe movement.")]
    [SerializeField] private bool _debugMode = false;

    private float _timer = 0f;
    private float _projectileTimer = 0f;

    private void Start()
    {
        if (!_debugMode)
        {
            StartCoroutine(StartAttack());
        }
    }

    private void Update()
    {
        if (_debugMode)
        {
            float sinValue = Mathf.Sin(_timer);
            transform.Translate(Vector2.right * _moveSpeed * (sinValue * Time.deltaTime));
            _timer += Time.deltaTime * _sineMoveStrength;
        }
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
        float strafeTimer = 0f;
        _timer = 0f;
        Vector2 strafeDirection = Vector2.right;
        if (Random.Range(0, 2) == 0) strafeDirection *= -1;
        while (_timer < _firingDuration)
        {
            // Strafe movement using sine.
            strafeTimer += Time.deltaTime * _sineMoveStrength;
            float sinValue = Mathf.Sin(strafeTimer);
            transform.Translate(strafeDirection * sinValue * _strafeSpeed * Time.deltaTime);

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
            transform.Translate(Vector2.up * Time.deltaTime * _exitSpeed);
            yield return null;
        }
    }
}
