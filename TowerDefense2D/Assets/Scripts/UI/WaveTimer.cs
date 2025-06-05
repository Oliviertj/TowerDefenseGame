using TMPro;
using UnityEngine;

public class WaveTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private float _waveTimer;
    private bool _isCountingDown = false;

    public void StartCountdown(float duration, float enemyCount)
    {
        _waveTimer = duration + enemyCount;
        _isCountingDown = true;
    }

    void Update()
    {
        if (!_isCountingDown) return;

        _waveTimer -= Time.deltaTime;

        if (_waveTimer <= 0f)
        {
            _waveTimer = 0f;
            _isCountingDown = false;
        }

        timerText.text = $"Volgende wave in: {_waveTimer:F0}s";
    }
}
