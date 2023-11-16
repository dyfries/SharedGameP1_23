using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalacticCrusadeProjectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [Header("Projectile Settings")]
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _projectileLifetime = 1f;

    private Vector2 _projectileDirection = Vector2.up;
    private float _lifeTimer = 0f;
    private bool _movementStarted = false;

    public void SetDirection(Vector2 direction)
    {
        _projectileDirection = direction;
    }

    private void Start()
    {
        _rb.velocity = _projectileDirection.normalized * _moveSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_lifeTimer >= _projectileLifetime)
        {
            DestroyProjectile();
        }
        else _lifeTimer += Time.deltaTime;
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
