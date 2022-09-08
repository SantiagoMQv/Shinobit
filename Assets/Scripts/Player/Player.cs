using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public HealthPlayer HealthPlayer { get; private set; }
    public ManaPlayer ManaPlayer { get; private set; }
    public PlayerAnimation playerAnimation { get; private set; }
    public PlayerJump playerJump { get; private set; }
    public UpgradeStats upgradeStats { get; private set; }
    public MovementPlayer movementPlayer { get; private set; }
    public CombatPlayer combatPlayer { get; private set; }

    public bool Healing { get; private set; }
    public bool NearToRespawn { get; private set; }

    private new void Awake()
    {
        HealthPlayer = GetComponent<HealthPlayer>();
        ManaPlayer = GetComponent<ManaPlayer>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerJump = GetComponent<PlayerJump>();
        upgradeStats = GetComponent<UpgradeStats>();
        movementPlayer = GetComponent<MovementPlayer>();
        combatPlayer = GetComponent<CombatPlayer>();
    }


    public void PlayerRestore()
    {
        HealthPlayer.PlayerRestore();
        ManaPlayer.ManaRestore();
        playerAnimation.PlayerRevive();
    }

    private void Update()
    {
        
        if(Inventary.Instance.healingNinjutsuItem != null) {
            if (Input.GetKeyDown(KeyCode.P) && !Healing && !playerJump.Jumping && !HealthPlayer.Defeated && Inventary.Instance.healingNinjutsuItem.currentNumTokens > 0 
                && HealthPlayer.CanBeHealed && !combatPlayer.Attacking)
            {
                Inventary.Instance.healingNinjutsuItem.UseItem();
            }
        }   


    }

    public void setNearToSpawn(bool state)
    {
        NearToRespawn = state;
    }

    public void setHealing(bool state)
    {
        Healing = state;
    }

    #region HealthToken

    IEnumerator HealWaiting()
    {
        Healing = true;
        yield return new WaitForSeconds(1f);
        Healing = false;
    }

    #endregion

    
}
