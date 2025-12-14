using UnityEngine;

public class BackGame : MonoBehaviour
{
    public GameObject canvasResume;
    public PlayerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasResume.SetActive(false);
    }
    public void ResumeGame()
    {
        playerController.IsPauseMenuOpen = !playerController.IsPauseMenuOpen;
        Time.timeScale = playerController.IsPauseMenuOpen ? 0f : 1f;  // Met en pause (0) ou reprend (1) le jeu
        canvasResume.SetActive(false);
    }
}