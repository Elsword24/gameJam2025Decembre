using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
