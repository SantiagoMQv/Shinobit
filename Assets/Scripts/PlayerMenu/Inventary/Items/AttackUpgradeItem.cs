using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UpgradeItem/AttackUpgrade")]
public class AttackUpgradeItem : UpgradeItem
{
    public override bool UseItem()
    {
        if (Pickups.Instance.CurrentBits >= bitsToUpgrade)
        {
            Inventary.Instance.Player.upgradeStats.UpgradeDamage();
            bitsToUpgrade *= multiplier;
            return true;
        }
        return false;
    }
}

