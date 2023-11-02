using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_StarWeavingOrb : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [Header("Projectile Settings")]
    [SerializeField] private float _delayBeforeMovement = 1f;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _projectileLifetime = 1f;

    private Vector2 _projectileDirection;
    private float _delayTimer = 0f;
    private float _lifeTimer = 0f;
    private bool _movementStarted = false;

    public void SetDirection(Vector2 direction)
    {
        _projectileDirection = direction;
    }

    private void Awake()
    {
        _delayTimer = _delayBeforeMovement;
        _lifeTimer = _projectileLifetime;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_delayTimer <= 0f)
        {
            // Start movement only once.
            if (!_movementStarted)
            {
                _rb.velocity = _projectileDirection.normalized * _speed;
            }
        }
        else _delayTimer -= Time.deltaTime;

        if (_lifeTimer <= 0f)
        {
            DestroyProjectile();
        }
        else _lifeTimer -= Time.deltaTime;
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
