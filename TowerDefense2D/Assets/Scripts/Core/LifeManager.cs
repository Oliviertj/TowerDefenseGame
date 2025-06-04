using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeManager : MonoBehaviour
{

    [SerializeField] private int _startingLives = 20;
    private int currentLives;

    private LivesUI _livesUI;
    public static LifeManager Instance { get; private set; }

    void Awake()
    {
        // Singleton pattern zodat andere scripts LifeManager kunnen bereiken
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentLives = _startingLives;
            _livesUI = FindObjectOfType<LivesUI>();
            _livesUI?.UpdateLivesDisplay();

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoseLives(float amount)
    {
        currentLives -= Mathf.RoundToInt(amount);
        Debug.Log($"Levens: {currentLives}");

        _livesUI?.UpdateLivesDisplay();

        if (currentLives <= 0)
        {
            GameOver();
        }
    }


    private void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene("GameOverScene");
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }
}
