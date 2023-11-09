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
            Vector2 randomSpawnPoint = new Vector2(Random.Range(0, _spawnArea.x), Random.Range(0, _spawnArea.y));
            Instantiate(_npcToSpawn, randomSpawnPoint, Quaternion.identity);
            _spawnTimer = 0f;
            _spawnCount++;
        }
        else _spawnTimer += Time.deltaTime;
    }

    public void InitializeSpawner(Vector2 newSpawnArea)
    {
        _spawnArea = newSpawnArea;
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
