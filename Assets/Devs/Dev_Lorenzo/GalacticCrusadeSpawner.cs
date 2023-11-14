using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalacticCrusadeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _npcToSpawn;
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _spawnDelay = 0f;
    [SerializeField] private int _maxSpawnCount = 1;

    [SerializeField] private Vector2 _spawnAreaCenter;        // Offsets from bounds center.
    [SerializeField] private Vector2 _boundsSize;
    private int _spawnCount = 0;
    private float _spawnTimer = 0f;


    private void Awake()
    {
        this.enabled = false;
    }

    private void Update()
    {
        if (_spawnCount >= _maxSpawnCount)
        {
            EndSpawner();
            return;
        }

        if (_spawnTimer >= _spawnInterval)
        {
            // Spawn units here.
            float xExtent = _boundsSize.x / 2f;
            Vector2 randomSpawnPoint = new Vector2(Random.Range(_spawnAreaCenter.x - xExtent, _spawnAreaCenter.y + xExtent), _spawnAreaCenter.y);
            Instantiate(_npcToSpawn, randomSpawnPoint, Quaternion.identity);
            // Set timers and counts.
            _spawnTimer = 0f;
            _spawnCount++;
        }
        else _spawnTimer += Time.deltaTime;
    }

    public void InitializeSpawner(Vector2 spawnAreaCenter, Vector2 boundsSize)
    {
        _spawnAreaCenter = spawnAreaCenter;
        _boundsSize = boundsSize;
    }

    public void StartSpawner()
    {
        this.enabled = true;
        _spawnCount = 0;
        _spawnTimer = 0f - _spawnDelay;
    }

    public void EndSpawner()
    {
        this.enabled = false;
    }
}
