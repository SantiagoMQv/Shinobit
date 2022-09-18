using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : Singleton<DeathManager>
{
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private Player player;

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
        player.PlayerRestore();

    }
}
