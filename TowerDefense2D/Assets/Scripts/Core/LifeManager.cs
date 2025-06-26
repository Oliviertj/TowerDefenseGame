using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Beheert de levens van de speler tijdens het spel. Verlaagt levens en toont Game Over.
/// Singleton patroon wordt gebruikt zodat andere scripts eenvoudig toegang hebben tot LifeManager,
/// zonder handmatig referenties door te geven.
/// </summary>
public class LifeManager : MonoBehaviour
{
    [SerializeField] private int _startingLives = 20;
    private int _currentLives;

    private LivesUI _livesUI;

    /// <summary>
    /// Singleton-instantie, zodat LifeManager universeel aanspreekbaar is in andere scripts.
    /// Gebruik van static, omdat het slechts 1 levensmanager nodig is voor de game.
    /// </summary>
    public static LifeManager Instance { get; private set; }

    void Awake()
    {
        // Singleton pattern zodat andere scripts LifeManager kunnen bereiken zonder losse referentie
        if (Instance == null)
        {
            Instance = this;

            // Houd dit object actief bij scenewissels zodat als ik opnieuw speel dat dit aanwezig is
            DontDestroyOnLoad(gameObject);

            _currentLives = _startingLives;

            _livesUI = FindObjectOfType<LivesUI>();
            _livesUI?.UpdateLivesDisplay();
        }
        else
        {
            // Zorgt ervoor dat er nooit meerdere LifeManagers bestaan
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Vermindert levens en checkt of de speler verloren heeft.
    /// </summary>
    /// <param name="amount">Aantal levens om af te trekken</param>
    public void LoseLives(float amount)
    {
        _currentLives -= Mathf.RoundToInt(amount);
        Debug.Log($"Levens: {_currentLives}");

        _livesUI?.UpdateLivesDisplay();

        if (_currentLives <= 0)
        {
            GameOver();
        }
    }

    /// <summary>
    /// Laadt de Game Over scene als alle levens op zijn.
    /// </summary>
    private void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene("GameOverScene");
    }

    /// <summary>
    /// Geeft huidig aantal levens terug voor de UI.
    /// </summary>
    /// <returns>Huidige levenswaarde</returns>
    public int GetCurrentLives()
    {
        return _currentLives;
    }
}
