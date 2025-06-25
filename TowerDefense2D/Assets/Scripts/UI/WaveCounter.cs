using TMPro;
using UnityEngine;

public class WaveCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;

    private int currentWave;

    // Methode om de huidige wave in te stellen en tekst bij te werken
    public void SetCurrentWave(int wave)
    {
        currentWave = wave +1;
        UpdateWaveText();
    }

    private void UpdateWaveText()
    {
        if (waveText != null)
        {
            waveText.text = $"Wave: {currentWave}";
        }
        else
        {
            Debug.LogWarning("WaveText is not assigned!");
        }
    }
}
