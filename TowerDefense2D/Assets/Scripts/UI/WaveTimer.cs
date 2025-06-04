using TMPro;
using UnityEngine;

public class WaveTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private float timer;
    private bool isCountingDown = false;

    public void StartCountdown(float duration)
    {
        timer = duration;
        isCountingDown = true;
    }

    void Update()
    {
        if (!isCountingDown) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = 0f;
            isCountingDown = false;
        }

        timerText.text = $"Volgende wave in: {timer:F1}s";
    }
}
