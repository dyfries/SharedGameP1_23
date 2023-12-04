using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_GalacticCrusade : Ability_Simple
{
    // On activation, starts bombardment.
    // Bombardment persists for a duration, independent from this ability (working similarly to a buff).
    [Header("--- Cosmic Bombardment Settings")]
    [Header("Spawners")]
    [SerializeField] private GalacticCrusadeSpawner[] _spawners;
    [Header("Level Bounds")]
    // TODO: Reference bounds from some other class that manages this info, and offset spawn area based on it.
    [SerializeField] private Vector2 _boundsCenter;        // Offsets from bounds center.
    [SerializeField] private Vector2 _boundsSize = Vector2.one;
    [SerializeField] private bool _previewSpawnAreaGizmo = false;

    private void Awake()
    {
        // Create and initialize spawners.
        foreach (GalacticCrusadeSpawner spawner in _spawners)
        {
            spawner.InitializeSpawner(_boundsCenter, _boundsSize);
        }
    }

    protected override void StartFiring()
    {
        base.StartFiring();
        foreach (GalacticCrusadeSpawner spawner in _spawners)
        {
            spawner.StartSpawner();
        }
    }

    protected override void StartWinddown()
    {
        base.StartWinddown();
    }

    private void OnDrawGizmos()
    {
        if (_previewSpawnAreaGizmo)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_boundsCenter, _boundsSize);
        }
    }
}
