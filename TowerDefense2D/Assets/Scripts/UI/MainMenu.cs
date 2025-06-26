using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    [SerializeField] private GameObject _guidePanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ShowGuide()
    {
        _guidePanel.SetActive(true);
    }

    public void CloseGuide()
    {
        _guidePanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
