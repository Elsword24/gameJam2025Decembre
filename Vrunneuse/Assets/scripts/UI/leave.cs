using UnityEngine;
public class Leave : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }
}