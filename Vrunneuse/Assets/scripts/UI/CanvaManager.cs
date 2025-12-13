using UnityEngine;

public class CanvaManager : MonoBehaviour
{
    public GameObject canvasMenuPrincipal;
    public GameObject canvasCredit;
    void Start()
    {
        canvasMenuPrincipal.SetActive(true);
        canvasCredit.SetActive(false);
    }

    public void SwitchToCredit()
    {
        Debug.Log("Go To Credit");
        canvasCredit.SetActive(true);
        canvasMenuPrincipal.SetActive(false);
    }

    public void SwitchToMenuPrincipal()
    {
        Debug.Log("Go To Menu Principal");
        canvasMenuPrincipal.SetActive(true);
        canvasCredit.SetActive(false);
        
    }
}
