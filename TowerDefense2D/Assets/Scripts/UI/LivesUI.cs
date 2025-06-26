using System.IO;
using TMPro;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _livesText;

    void Start()
    {
        if (_livesText == null)
        {
            Debug.LogError("Geen LivesText component in Canvas gevonden!");
            return;
        }

        UpdateLivesDisplay();
    }

    public void UpdateLivesDisplay()
    {
        if (_livesText != null && LifeManager.Instance != null)
        {
            _livesText.text = $"Levens: {LifeManager.Instance.GetCurrentLives()}";
        }
    }
}
