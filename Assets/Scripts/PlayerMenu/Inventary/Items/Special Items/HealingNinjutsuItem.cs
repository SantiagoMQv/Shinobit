using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Items/HealingNinjutsu")]
public class HealingNinjutsuItem : InventaryItem
{
    [Header("General info")]
    public float InitialHPRestoration;

    public override bool UseItem()
    {
        Player.Instance.combatPlayer.RemoveHealthToken();
        Player.Instance.HealthPlayer.RestoreHealth(Player.Instance.combatPlayer.GetHpHPRestoration());
        Player.Instance.combatPlayer.StartCoroutine("HealWaiting");
        return true;
    }

}
