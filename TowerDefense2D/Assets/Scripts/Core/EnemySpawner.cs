using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 2f;

    private float spawnTimer;
    private List<Vector2> path;

    void Start()
    {
        // Haal het pad op
        path = FindObjectOfType<Map>()?.GetPath();

        if (path == null || path.Count == 0)
        {
            Debug.LogError("Geen geldig pad gevonden!");
            return;
        }

        // Zet de spawner op de eerste tile van het pad
        transform.position = path[0];
    }

    void Update()
    {
        if (path == null || path.Count == 0) return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        GameObject enemyObj = Instantiate(enemyPrefab, path[0], Quaternion.identity);
        EnemyBase enemy = enemyObj.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            enemy.SetPath(path);
        }
    }
}
