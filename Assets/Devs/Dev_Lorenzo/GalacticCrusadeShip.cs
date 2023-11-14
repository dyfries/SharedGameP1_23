using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalacticCrusadeShip : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _projectileSpawnPoint;
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _initialMovementDuration = 0.5f;
    [SerializeField] private float _firingDuration = 3f;
    [SerializeField] private float _exitLifetime = 10f;
    [SerializeField] private float _sineMoveStrength = 1f;
    private void Update()
    {
        transform.Translate(Vector2.up * Time.deltaTime * 5f);
    }
}
