using UnityEngine;
public class Leave : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false; // Décommente pour tester dans l'Editor
    }
}