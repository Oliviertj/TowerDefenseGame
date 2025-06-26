using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckWinCondition : MonoBehaviour
{
   [SerializeField] private WaveManager waveManager;

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
