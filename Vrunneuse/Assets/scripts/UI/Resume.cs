using UnityEngine;

public class Resume : MonoBehaviour
{
    public GameObject canvasResume;
    public PlayerController playerController;
    void Start()
    {
        canvasResume.SetActive(false);
    }

    public void ResumeGame()
    {
        canvasResume.SetActive(false);
    }
}
