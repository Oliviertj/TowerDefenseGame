using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Setup")]
    [SerializeField] private List<GameObject> enemyPrefabs;  // meerdere vijand-types
    [SerializeField] private float spawnInterval = 2f;

    private float spawnTimer;
    private List<Vector2> path;

    void Start()
    {
        path = FindObjectOfType<Map>()?.GetPath();

        if (path == null || path.Count == 0)
        {
            Debug.LogError("Geen geldig pad gevonden!");
            return;
        }

        transform.position = path[0]; // zet spawner op eerste padpunt
    }

    void Update()
    {
        if (path == null || path.Count == 0) return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnRandomEnemy();
            spawnTimer = spawnInterval;
        }
    }

    void SpawnRandomEnemy()
    {
        if (enemyPrefabs.Count == 0)
        {
            Debug.LogWarning("Geen enemy prefabs gekoppeld!");
            return;
        }

        // Kies willekeurige prefab
        int index = Random.Range(0, enemyPrefabs.Count);
        GameObject selectedPrefab = enemyPrefabs[index];

        // Spawn enemy op het eerste padpunt
        Vector3 spawnPos = new Vector3(path[0].x, path[0].y, -1f); // Z-positie iets lager dan de tiles
        GameObject enemyObj = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
        EnemyBase enemy = enemyObj.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            enemy.SetPath(path);
        }
    }
}
