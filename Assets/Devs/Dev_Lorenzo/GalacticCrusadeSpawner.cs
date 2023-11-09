using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalacticCrusadeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _npcToSpawn;
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _spawnDelay = 0f;
    [SerializeField] private int _maxSpawnCount = 1;

    private Vector2 _spawnArea;
    private float _spawnTimer = 0f;

    public void InitializeSpawner(Vector2 newSpawnArea)
    {
        _spawnArea = newSpawnArea;
    }

    private void Update()
    {
        if (_spawnTimer >= _spawnInterval)
        {
            // Spawn units here.
            _spawnTimer = 0f;
        }
        else _spawnTimer += Time.deltaTime;
    }

    public void StartSpawner()
    {
        this.enabled = true;
        _spawnTimer = 0f;
    }

    public void EndSpawner()
    {
        this.enabled = false;
    }
}
