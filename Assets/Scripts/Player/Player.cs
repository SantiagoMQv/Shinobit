using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public HealthPlayer HealthPlayer { get; private set; }
    public ManaPlayer ManaPlayer { get; private set; }
    public PlayerAnimation playerAnimation { get; set; }
    private PlayerJump playerJump;
    public bool Healing { get; private set; }

    private void Awake()
    {
        HealthPlayer = GetComponent<HealthPlayer>();
        ManaPlayer = GetComponent<ManaPlayer>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerJump = GetComponent<PlayerJump>();
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
            if (Input.GetKeyDown(KeyCode.P) && !Healing && !playerJump.Jumping && !HealthPlayer.Defeated && Inventary.Instance.healingNinjutsuItem.currentNumTokens > 0)
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
