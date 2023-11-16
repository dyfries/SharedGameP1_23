using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private List<float> _xSpawnPositions = new();       // X positions are cached to spawn NPCs randomly and evenly.


    private void Awake()
    {
        this.enabled = false;

        // Generate X positions.
        for (float i = 0; i < +_maxSpawnCount; i++)
        {
            float startingXPos = _spawnAreaCenter.x - (_boundsSize.x / 2f);
            float xMultiplier = i / (_maxSpawnCount - 1);
            float xPosition = startingXPos + (_boundsSize.x * xMultiplier);
            _xSpawnPositions.Add(xPosition);
        }
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
            float xPosition = _xSpawnPositions[_spawnCount];
            float yPosition = -_boundsSize.y / 2f;
            Vector2 randomSpawnPosition = new Vector2(xPosition, yPosition);
            // OLD SPAWNING METHOD
            
            //float xExtent = _boundsSize.x / 2f;
            //float yPosition = -_boundsSize.y / 2f;
            //Vector2 randomSpawnPosition = new Vector2(Random.Range(_spawnAreaCenter.x - xExtent, _spawnAreaCenter.y + xExtent), yPosition);
            
            Instantiate(_npcToSpawn, randomSpawnPosition, Quaternion.identity);
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
        ShuffleXPositions();
    }

    public void EndSpawner()
    {
        this.enabled = false;
    }

    private void ShuffleXPositions()
    {
        System.Random rng = new System.Random();
        _xSpawnPositions = _xSpawnPositions.OrderBy((x) => rng.Next()).ToList();
    }
}
