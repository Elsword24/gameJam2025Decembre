using UnityEngine;
public class Leave : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
    }

    public void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false; // Décommente pour tester dans l'Editor
    }
}