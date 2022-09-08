using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UpgradeItem/DefenseUpgrade")]
public class DefenseUpgradeItem : UpgradeItem
{
    public override bool UseItem()
    {
        if (Pickups.Instance.CurrentBits >= bitsToUpgrade)
        {
            Inventary.Instance.Player.upgradeStats.UpgradeDefense();
            bitsToUpgrade *= multiplier;
            return true;
        }
        return false;
    }
}
