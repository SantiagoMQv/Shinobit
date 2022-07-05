using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UpgradeItem/AttackUpgrade")]
public class AttackUpgradeItem : UpgradeItem
{
    public override bool UseItem()
    {
        if (Inventary.Instance.CurrentBits >= bitsToUpgrade)
        {
            Inventary.Instance.Player.PlayerStats.Damage++;
            bitsToUpgrade *= multiplier;
            return true;
        }
        return false;
    }
}

