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
    [SerializeField] private GameObject _slimeyPrefab;
    [SerializeField] private GameObject _gloopPrefab;
    [SerializeField] private GameObject _splattyPrefab;


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
                    wave.enemiesInThisWave.Add(_slimeyPrefab);
            }
            else if (i == 1)
            {
                // Tweede wave: 3 slimey + 2 gloop
                for (int j = 0; j < 3; j++)
                    wave.enemiesInThisWave.Add(_slimeyPrefab);
                for (int j = 0; j < 2; j++)
                    wave.enemiesInThisWave.Add(_gloopPrefab);
            }
            else
            {
                // Andere waves: random mix
                for (int j = 0; j < 6 + i; j++)
                {
                    int type = Random.Range(0, 3);
                    if (type == 0) wave.enemiesInThisWave.Add(_slimeyPrefab);
                    else if (type == 1) wave.enemiesInThisWave.Add(_gloopPrefab);
                    else wave.enemiesInThisWave.Add(_splattyPrefab);
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

            // Wacht tot de timer afloopt (resterende tijd vanaf de wave start)
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
