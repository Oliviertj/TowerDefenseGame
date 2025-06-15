using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Button _startButton;

    [Header("Wave Config")]
    [SerializeField] private List<Wave> _waves = new List<Wave>();
    [SerializeField] private float _timeBetweenSpawns = 1f;
    [SerializeField] private float _timeBetweenWaves = 5f;

    [Header("Vaste Enemy voor Wave 1")]
    [SerializeField] private GameObject _slimeyPrefab;

    [Header("Enemy Types")]
    [SerializeField] private List<GameObject> _enemyPrefabs = new List<GameObject>();

    private int _currentWaveIndex = 0;
    private List<Vector2> _path;

    private WaveTimer _waveTimer;

    void Start()
    {
        _path = FindObjectOfType<Map>()?.GetPath();
        _waveTimer = FindObjectOfType<WaveTimer>();

        if (_path == null || _path.Count == 0)
        {
            Debug.LogError("Geen pad gevonden!");
            return;
        }

        if (_enemyPrefabs.Count == 0)
        {
            Debug.LogWarning("Geen enemy prefabs ingesteld! Voeg vijanden toe in de Inspector.");
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
                // Eerste wave: 5 vaste slimeys
                for (int j = 0; j < 5; j++)
                {
                    if (_slimeyPrefab != null)
                        wave.enemiesInThisWave.Add(_slimeyPrefab);
                }
            }
            else
            {
                float targetWeight = 6 + i;
                float currentWeight = 0f;

                while (currentWeight < targetWeight && _enemyPrefabs.Count > 0)
                {
                    GameObject selected = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)];

                    // Haal weight op uit EnemyBase component op de prefab zelf
                    EnemyBase enemyBase = selected.GetComponent<EnemyBase>();
                    if (enemyBase == null)
                    {
                        Debug.LogWarning($"{selected.name} mist EnemyBase component!");
                        continue;
                    }

                    float weight = enemyBase.EnemyWeight;
                    currentWeight += weight;
                    wave.enemiesInThisWave.Add(selected);
                }
            }

            _waves.Add(wave);
        }

        Debug.Log("Waves gegenereerd met weights via EnemyBase.");
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
        while (_currentWaveIndex < _waves.Count)
        {
            Debug.Log($"Start ronde {_currentWaveIndex + 1}");

            Wave currentWave = _waves[_currentWaveIndex];

            // Start visuele timer meteen bij start van de wave
            if (_waveTimer != null)
                _waveTimer.StartCountdown(_timeBetweenWaves, currentWave.enemiesInThisWave.Count);

            float waveTimeRemaining = _timeBetweenWaves;

            foreach (var enemyPrefab in currentWave.enemiesInThisWave)
            {
                SpawnEnemy(enemyPrefab);
                yield return new WaitForSeconds(_timeBetweenSpawns);
            }

            _currentWaveIndex++;

            while (waveTimeRemaining > 0f)
            {
                waveTimeRemaining -= Time.deltaTime;
                yield return null;
            }
        }

        Debug.Log("Alle rondes voltooid!");
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector3 spawnPos = new Vector3(_path[0].x, _path[0].y, -1f);
        GameObject enemyObj = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        if (enemyObj.TryGetComponent(out EnemyBase enemy))
        {
            enemy.SetPath(_path);
        }
    }
}
