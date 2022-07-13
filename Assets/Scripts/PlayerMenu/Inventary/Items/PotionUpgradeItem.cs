using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UpgradeItem/PotionUpgrade")]
public class PotionUpgradeItem : UpgradeItem
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
