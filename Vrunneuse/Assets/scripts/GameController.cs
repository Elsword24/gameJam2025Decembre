using NUnit.Framework;
using System.IO;
using UnityEngine;
using System.Collections.Generic;


public class GameController : MonoBehaviour
{
    //Player start coordonnées
    public PlayerController playerController;
    public AvatarController avatar;
    private bool isFirstRun = true;
    //Stockage des mouvements
    //fin de tableau
    void Start()
    {
        //playerController = GetComponent<PlayerController>();

        if (isFirstRun)
        {
            string path = Application.persistentDataPath + "defaultPath.json";
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                PlayerController.ActionWrapper wrapper = JsonUtility.FromJson<PlayerController.ActionWrapper>(json);
                avatar.StartReplay(wrapper.actions);
            }
            PlayerPrefs.SetInt("FirstRun", 0);
        } else
        {
            isFirstRun = false;
        }
    }

    public void OnPlayerRespawn()
    {
        List<ActionData> lastRun = new List<ActionData>( playerController.StopRecording());
        avatar.StartReplay(lastRun);
        playerController.StartRecording();
    }


}
