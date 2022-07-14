using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public HealthPlayer HealthPlayer { get; private set; }
    public ManaPlayer ManaPlayer { get; private set; }
    public PlayerAnimation playerAnimation { get; set; }
    public PlayerJump playerJump { get; set; }
    public NearToSaveAltar saveAltar { get; set; }
    public UpgradeStats upgradeStats { get; set; }
    public bool Healing { get; private set; }
    public bool NearToRespawn { get; private set; }
    private void Awake()
    {
        HealthPlayer = GetComponent<HealthPlayer>();
        ManaPlayer = GetComponent<ManaPlayer>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerJump = GetComponent<PlayerJump>();
        upgradeStats = GetComponent<UpgradeStats>();
        saveAltar = GetComponent<NearToSaveAltar>();
    }


    public void PlayerRestore()
    {
        HealthPlayer.PlayerRestore();
        ManaPlayer.ManaRestore();
        playerAnimation.PlayerRevive();
    }

    private void Update()
    {
        // Uso de HealingNinjutsu
        if(Inventary.Instance.healingNinjutsuItem != null) {
            if (Input.GetKeyDown(KeyCode.P) && !Healing && !playerJump.Jumping && !HealthPlayer.Defeated && Inventary.Instance.healingNinjutsuItem.currentNumTokens > 0 
                && HealthPlayer.CanBeHealed)
            {
                RemoveHealthToken();
                HealthPlayer.RestoreHealth(Inventary.Instance.healingNinjutsuItem.currentHPRestoration);
                StartCoroutine("HealWaiting");
            }
        }
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            AddAllHealthToken();
        }

    }

    public void setNearToSpawn(bool state)
    {
        NearToRespawn = state;
    }

    #region HealthToken
    public void RemoveHealthToken()
    {
        if (Inventary.Instance.healingNinjutsuItem.currentNumTokens > 0)
        {
            Inventary.Instance.healingNinjutsuItem.currentNumTokens = UIManager.Instance.RemoveHealthTokenUI() - 1;
        }
    }

    public void AddAllHealthToken()
    {
        if(Inventary.Instance.healingNinjutsuItem != null)
        {
            if (Inventary.Instance.healingNinjutsuItem.currentNumTokens < 8)
            {

                for (int i = Inventary.Instance.healingNinjutsuItem.currentNumTokens; i < Inventary.Instance.healingNinjutsuItem.InitialTokens; i++)
                {
                    Inventary.Instance.healingNinjutsuItem.currentNumTokens = UIManager.Instance.AddHealthTokenUI();
                }

            }
        }
    }

    IEnumerator HealWaiting()
    {
        Healing = true;
        yield return new WaitForSeconds(1f);
        Healing = false;
    }

    // Al coger el HealingNinjutsu los niveles actuales de Tokens y curación se inicializan
    private void PickupHealingNinjutsuItemResponse()
    {
        Inventary.Instance.healingNinjutsuItem.currentNumTokens = Inventary.Instance.healingNinjutsuItem.InitialTokens;
        Inventary.Instance.healingNinjutsuItem.currentHPRestoration = Inventary.Instance.healingNinjutsuItem.InitialHPRestoration;
    }

    #endregion

    private void OnEnable()
    {
        Inventary.PickupHealingNinjutsuItemEvent += PickupHealingNinjutsuItemResponse;
    }

    private void OnDisable()
    {
        Inventary.PickupHealingNinjutsuItemEvent -= PickupHealingNinjutsuItemResponse;
    }
}
