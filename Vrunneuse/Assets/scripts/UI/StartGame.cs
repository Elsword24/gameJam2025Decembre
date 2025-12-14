using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{

    public PlayerController playerController;
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void BackScene(string BackSceneName)
    {
        playerController.IsPauseMenuOpen = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(BackSceneName);
    }
}