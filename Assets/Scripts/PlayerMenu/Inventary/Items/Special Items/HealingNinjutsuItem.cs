using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Items/HealingNinjutsu")]
public class HealingNinjutsuItem : InventaryItem
{
    [Header("General info")]
    public float InitialHPRestoration;
    public int InitialTokens;
    [HideInInspector] public int currentNumTokens;
    [HideInInspector] public float currentHPRestoration;

    public override bool UseItem()
    {
        RemoveHealthToken();
        Player.Instance.HealthPlayer.RestoreHealth(Inventary.Instance.healingNinjutsuItem.currentHPRestoration);
        Player.Instance.StartCoroutine("HealWaiting");
        return true;
    }

    public void RemoveHealthToken()
    {
        if (Inventary.Instance.healingNinjutsuItem.currentNumTokens > 0)
        {
            Inventary.Instance.healingNinjutsuItem.currentNumTokens = UIManager.Instance.RemoveHealthTokenUI() - 1;
        }
    }

    public void AddAllHealthToken()
    {
        if (Inventary.Instance.healingNinjutsuItem != null)
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

    // Al coger el HealingNinjutsu los niveles actuales de Tokens y curación se inicializan
    private void PickupHealingNinjutsuItemResponse()
    {
        currentNumTokens = InitialTokens;
        currentHPRestoration = InitialHPRestoration;
    }

    private void OnEnable()
    {
        Inventary.PickupHealingNinjutsuItemEvent += PickupHealingNinjutsuItemResponse;
    }

    private void OnDisable()
    {
        Inventary.PickupHealingNinjutsuItemEvent -= PickupHealingNinjutsuItemResponse;
    }
}
