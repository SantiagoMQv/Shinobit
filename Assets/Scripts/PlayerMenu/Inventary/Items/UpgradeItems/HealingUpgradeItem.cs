using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UpgradeItem/HealingUpgrade")]
public class HealingUpgradeItem : UpgradeItem
{
    public override bool UseItem()
    {
        if(Pickups.Instance.CurrentBits >= bitsToUpgrade)
        {
            Inventary.Instance.Player.upgradeStats.UpgradePotion();
            bitsToUpgrade *= multiplier;
            return true;
        }
        return false;
    }
}
