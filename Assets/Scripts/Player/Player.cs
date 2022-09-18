using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>, ISaveGame
{
    [SerializeField] private PlayerStats stats;

    public HealthPlayer HealthPlayer { get; private set; }
    public ManaPlayer ManaPlayer { get; private set; }
    public StaminaPlayer StaminaPlayer { get; private set; }
    public PlayerAnimation playerAnimation { get; private set; }
    public PlayerJump playerJump { get; private set; }
    public UpgradeStats upgradeStats { get; private set; }
    public MovementPlayer movementPlayer { get; private set; }
    public CombatPlayer combatPlayer { get; private set; }

    public bool NearToRespawn { get; private set; }
    public PlayerStats Stats => stats;

    [HideInInspector] public Vector3 respawnPosition;
    [HideInInspector] public int respawnSceneIndex;

    private new void Awake()
    {
        HealthPlayer = GetComponent<HealthPlayer>();
        ManaPlayer = GetComponent<ManaPlayer>();
        StaminaPlayer = GetComponent<StaminaPlayer>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerJump = GetComponent<PlayerJump>();
        upgradeStats = GetComponent<UpgradeStats>();
        movementPlayer = GetComponent<MovementPlayer>();
        combatPlayer = GetComponent<CombatPlayer>();

        respawnPosition = new Vector3(0,0,0);


        NearToRespawn = false;
    }

    public void LoadData(GameData data)
    {
        // Position
        this.transform.position = data.respawnPosition;

        // Respawn
        this.respawnPosition = data.respawnPosition;
        this.respawnSceneIndex = data.respawnSceneIndex;

        // Stats
        stats.Damage = data.playerStatsData.Damage;
        stats.Defense = data.playerStatsData.Defense;
        stats.Potion = data.playerStatsData.Potion;
        stats.HealthPoints = data.playerStatsData.HealthPoints;
        stats.SpellPoints = data.playerStatsData.SpellPoints;
        stats.StaminaPoints = data.playerStatsData.StaminaPoints;
    }
    public void SaveData(ref GameData data)
    {
        // Respawn
        data.respawnPosition = this.respawnPosition;
        data.respawnSceneIndex = this.respawnSceneIndex;

        // Stats
        data.playerStatsData.Damage = stats.Damage;
        data.playerStatsData.Defense = stats.Defense;
        data.playerStatsData.Potion = stats.Potion;
        data.playerStatsData.HealthPoints = stats.HealthPoints;
        data.playerStatsData.SpellPoints = stats.SpellPoints;
        data.playerStatsData.StaminaPoints = stats.StaminaPoints;
    }

    public void PlayerRestore()
    {
        DeathManager.Instance.CloseDeathPanel();
        HealthPlayer.PlayerRestore();
        ManaPlayer.ManaRestore();
        StaminaPlayer.StaminaRestore();
        playerAnimation.PlayerRevive();
    }

    public void setNearToSpawn(bool state)
    {
        NearToRespawn = state;
    }




    
}
