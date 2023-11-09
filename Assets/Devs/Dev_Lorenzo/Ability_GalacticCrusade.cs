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
    [Header("Spawn Area Settings")]
    // TODO: Reference bounds from some other class that manages this info, and offset spawn area based on it.
    [SerializeField] private Vector2 _spawnAreaCenter;        // Offsets from bounds center.
    [SerializeField] private int _spawnAreaWidth = 1;
    [SerializeField] private bool _previewSpawnAreaGizmo = false;

    private List<GalacticCrusadeSpawner> _spawnerInstances = new();

    private void Awake()
    {
        // TODO: Spawners should already be part of this ability prefab.
        // Create and initialize spawners.
        foreach (GalacticCrusadeSpawner spawner in _spawners)
        {
            GalacticCrusadeSpawner spawnerInstance = Instantiate(spawner, gameObject.transform);
            spawnerInstance.InitializeSpawner(_spawnAreaCenter, _spawnAreaWidth);
            _spawnerInstances.Add(spawnerInstance);
        }
    }

    protected override void StartFiring()
    {
        base.StartFiring();
        foreach (GalacticCrusadeSpawner spawner in _spawnerInstances)
        {
            spawner.StartSpawner();
        }
    }

    protected override void StartWinddown()
    {
        base.StartWinddown();
        // Just to be safe.
        StopAllCoroutines();
    }

    private void OnDrawGizmos()
    {
        if (_previewSpawnAreaGizmo)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_spawnAreaCenter, new Vector2(_spawnAreaWidth, 0.5f));
        }
    }
}
