using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckWinCondition : MonoBehaviour
{
    public WaveManager waveManager; // Sleep hier je WaveManager in de Inspector

    private bool _winTriggered = false;

    void Update()
    {
        if (_winTriggered || waveManager == null)
            return;

        if (waveManager.IsFinalWave && GameObject.FindObjectsOfType<EnemyBase>().Length == 0)
        {
            _winTriggered = true;
            SceneManager.LoadScene("WinScene");
        }
    }
}
