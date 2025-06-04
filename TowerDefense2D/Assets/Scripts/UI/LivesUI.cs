using System.IO;
using TMPro;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI livesText;

    void Start()
    {
        if (livesText == null)
        {
            Debug.LogError("Geen LivesText component in Canvas gevonden!");
            return;
        }

        UpdateLivesDisplay();
    }

    public void UpdateLivesDisplay()
    {
        if (livesText != null && LifeManager.Instance != null)
        {
            livesText.text = $"Levens: {LifeManager.Instance.GetCurrentLives()}";
        }
    }
}
