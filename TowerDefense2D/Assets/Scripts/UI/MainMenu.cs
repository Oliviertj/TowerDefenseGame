using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public GameObject guidePanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ShowGuide()
    {
        guidePanel.SetActive(true);
    }

    public void CloseGuide()
    {
        guidePanel.SetActive(false);
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
