using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class DeathManager : Singleton<DeathManager>
{
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private Player player;

    public static Action ReviveLoadSceneAction;
    public void OpenDeathPanel()
    {
        deathPanel.SetActive(true);
    }

    public void CloseDeathPanel()
    {
        deathPanel.SetActive(false);
    }

    public void ReviveButton()
    {
        SaveManager.Instance.LoadGame();
        ReviveLoadSceneAction?.Invoke();
        SceneManager.LoadScene(Player.Instance.respawnSceneIndex);
       

    }
}
