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
    [SerializeField] private int _numberOfWaves = 10;

    [Header("Vaste Enemy voor Wave 1")]
    [SerializeField] private GameObject _slimeyPrefab;

    [Header("Enemy Types")]                                
    [SerializeField] private List<GameObject> _enemyIntroductions = new List<GameObject>();
    [SerializeField] private GameObject _superSlimePrefab;



    private int _currentWaveIndex = 0;
    private List<Vector2> _path;

    private WaveTimer _waveTimer;
    private WaveCounter _waveCounter;

    private Wave finalBossWave;
    public bool IsFinalWave => _currentWaveIndex >= _waves.Count;

    void Start()                
    {
        _path = FindObjectOfType<Map>()?.GetPath();
        _waveTimer = FindObjectOfType<WaveTimer>();
        _waveCounter = FindObjectOfType<WaveCounter>();

        if (_path == null || _path.Count == 0)
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
        List<GameObject> unlockedEnemies = new List<GameObject> { _slimeyPrefab };

        for (int i = 0; i < _numberOfWaves; i++)

        {
            Wave wave = new Wave();
            wave.enemiesInThisWave = new List<GameObject>();

            float targetWeight = 6 + i+1;
            float currentWeight = 0f;

            bool hasNewIntro = i > 0 && (i - 1) < _enemyIntroductions.Count;
            GameObject newEnemy = hasNewIntro ? _enemyIntroductions[i - 1] : null;

            if (i == 0)
            {
                // Wave 1 = alleen slimey
                for (int j = 0; j < 5; j++)
                {
                    wave.enemiesInThisWave.Add(_slimeyPrefab);
                }
            }
            else
            {
                // Voeg nieuwe enemy 1x toe (alleen tijdens introductie)
                if (hasNewIntro && newEnemy != null)
                {
                    wave.enemiesInThisWave.Add(newEnemy);
                    float introWeight = GetWeightOfEnemy(newEnemy);
                    currentWeight += introWeight;

                    unlockedEnemies.Add(newEnemy); // Vanaf volgende wave beschikbaar
                }

                // Vul de rest van de wave aan
                while (currentWeight < targetWeight)
                {
                    GameObject selected = unlockedEnemies[Random.Range(0, unlockedEnemies.Count)];
                    float weight = GetWeightOfEnemy(selected);

                    if (currentWeight + weight > targetWeight) break;

                    wave.enemiesInThisWave.Add(selected);
                    currentWeight += weight;
                }
            }

            _waves.Add(wave);
        }
        if (_superSlimePrefab != null)
        {
            finalBossWave = new Wave();
            finalBossWave.enemiesInThisWave = new List<GameObject> { _superSlimePrefab };
            _waves.Add(finalBossWave);
        }

        Debug.Log("Waves gegenereerd met introductie en gewichtslimiet.");
    }

    private float GetWeightOfEnemy(GameObject enemyPrefab)
    {
        GameObject temp = Instantiate(enemyPrefab);
        float weight = 1f;

        if (temp.TryGetComponent(out EnemyBase enemy))
            weight = enemy.EnemyWeight;

        Destroy(temp);
        return weight;
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
                _waveCounter.SetCurrentWave(_currentWaveIndex);

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
