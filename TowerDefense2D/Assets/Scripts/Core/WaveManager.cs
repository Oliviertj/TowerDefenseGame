using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Configuratie")]
    [SerializeField] private List<Wave> waves = new List<Wave>();
    [SerializeField] private float timeBetweenSpawns = 1f;
    [SerializeField] private float timeBetweenWaves = 5f;

    private int currentWaveIndex = 0;
    private List<Vector2> path;

    void Start()
    {
        path = FindObjectOfType<Map>()?.GetPath();

        if (path == null || path.Count == 0)
        {
            Debug.LogError("Geen pad gevonden!");
            return;
        }

        StartCoroutine(RunWaves());
    }

    [System.Serializable]
    public class Wave
    {
        public List<GameObject> enemiesInThisWave;
    }

    IEnumerator RunWaves()
    {
        while (currentWaveIndex < waves.Count)
        {
            Debug.Log($"Start ronde {currentWaveIndex + 1}");

            Wave currentWave = waves[currentWaveIndex];

            foreach (var enemyPrefab in currentWave.enemiesInThisWave)
            {
                SpawnEnemy(enemyPrefab);
                yield return new WaitForSeconds(timeBetweenSpawns);
            }

            currentWaveIndex++;
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        Debug.Log("Alle rondes voltooid!");
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector3 spawnPos = new Vector3(path[0].x, path[0].y, -1f);
        GameObject enemyObj = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        if (enemyObj.TryGetComponent(out EnemyBase enemy))
        {
            enemy.SetPath(path);
        }
    }
}
