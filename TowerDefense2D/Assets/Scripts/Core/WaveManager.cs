using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaveManager : MonoBehaviour
{
    [SerializeField] private Button _startButton;

    [Header("Wave Configuratie")]
    [SerializeField] private List<Wave> _waves = new List<Wave>();
    [SerializeField] private float _timeBetweenSpawns = 1f;
    [SerializeField] private float _timeBetweenWaves = 5f;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject slimeyPrefab;
    [SerializeField] private GameObject gloopPrefab;
    [SerializeField] private GameObject splattyPrefab;


    private int currentWaveIndex = 0;
    private List<Vector2> path;

    private WaveTimer waveTimer;


    void Start()
    {
        path = FindObjectOfType<Map>()?.GetPath();
        waveTimer = FindObjectOfType<WaveTimer>();

        if (path == null || path.Count == 0)
        {
            Debug.LogError("Geen pad gevonden!");
            return;
        }

        if (_startButton != null)
        {
            _startButton.gameObject.SetActive(true);
            _startButton.onClick.AddListener(StartWaves);
        }

        GenerateWaves();
    }

    private void GenerateWaves()
    {
        _waves.Clear();

        for (int i = 0; i < 8; i++)
        {
            Wave wave = new Wave();
            wave.enemiesInThisWave = new List<GameObject>();

            if (i == 0)
            {
                // Eerste wave: 5 slimeys
                for (int j = 0; j < 5; j++)
                    wave.enemiesInThisWave.Add(slimeyPrefab);
            }
            else if (i == 1)
            {
                // Tweede wave: 3 slimey + 2 gloop
                for (int j = 0; j < 3; j++)
                    wave.enemiesInThisWave.Add(slimeyPrefab);
                for (int j = 0; j < 2; j++)
                    wave.enemiesInThisWave.Add(gloopPrefab);
            }
            else
            {
                // Andere waves: random mix
                for (int j = 0; j < 6 + i; j++)
                {
                    int type = Random.Range(0, 3);
                    if (type == 0) wave.enemiesInThisWave.Add(slimeyPrefab);
                    else if (type == 1) wave.enemiesInThisWave.Add(gloopPrefab);
                    else wave.enemiesInThisWave.Add(splattyPrefab);
                }
            }

            _waves.Add(wave);
        }

        Debug.Log("Waves gegenereerd via script.");
    }


    [System.Serializable]
    public class Wave
    {
        public List<GameObject> enemiesInThisWave;
    }
    private void StartWaves()
    {
        if (_startButton != null)
            _startButton.gameObject.SetActive(false); // Verberg de button.

        StartCoroutine(RunWaves());
    }


    IEnumerator RunWaves()
    {
        while (currentWaveIndex < _waves.Count)
        {
            Debug.Log($"Start ronde {currentWaveIndex + 1}");

            Wave currentWave = _waves[currentWaveIndex];

            foreach (var enemyPrefab in currentWave.enemiesInThisWave)
            {
                SpawnEnemy(enemyPrefab);
                yield return new WaitForSeconds(_timeBetweenSpawns);
            }

            currentWaveIndex++;
            if (waveTimer != null)
                waveTimer.StartCountdown(_timeBetweenWaves);

            float wait = _timeBetweenWaves;
            while (wait > 0f)
            {
                wait -= Time.deltaTime;
                yield return null;
            }
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
