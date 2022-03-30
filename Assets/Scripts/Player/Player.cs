using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public HealthPlayer HealthPlayer { get; private set; }
    public ManaPlayer ManaPlayer { get; private set; }
    public PlayerAnimation playerAnimation { get; set; }

    private void Awake()
    {
        HealthPlayer = GetComponent<HealthPlayer>();
        ManaPlayer = GetComponent<ManaPlayer>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    
    public void PlayerRestore()
    {
        HealthPlayer.PlayerRestore();
        ManaPlayer.ManaRestore();
        playerAnimation.PlayerRevive();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }
}
