using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_GalacticCrusade : Ability_Simple
{
    // On activation, starts bombardment.
    // Bombardment persists for a duration, independent from this ability (working similarly to a buff).
    [Header("--- Cosmic Bombardment Settings")]
    [Header("Spawn Settings")]
    // TODO: Reference bounds from some other class that manages this info, and offset spawn area based on it.
    [SerializeField] private Vector2 _spawnAreaPosition;        // Offsets from bounds center.
    [SerializeField] private int _spawnAreaWidth = 1;
    [SerializeField] private bool _previewSpawnAreaGizmo = false;

    private float _projectileTimer = 0;

    protected override void StartFiring()
    {
        base.StartFiring();
        _projectileTimer = 0f;
        //StartCoroutine(StartAbilityDash());
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
            Gizmos.DrawWireCube(_spawnAreaPosition, new Vector2(_spawnAreaWidth, 0.5f));
        }
    }
}
