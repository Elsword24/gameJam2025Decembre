using UnityEngine;
using UnityEngine.UI;

public class CanvaSwitcher : MonoBehaviour
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
        canvasMenuPrincipal.SetActive(false);
        canvasCredit.SetActive(true);
    }

    public void SwitchToMenuPrincipale()
    {
        Debug.Log("Go To Menu");
        canvasCredit.SetActive(false);
        canvasMenuPrincipal.SetActive(true);
    }
}