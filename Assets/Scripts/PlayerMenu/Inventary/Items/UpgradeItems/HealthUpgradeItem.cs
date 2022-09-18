using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UpgradeItem/HealthUpgrade")]
public class HealthUpgradeItem : UpgradeItem
{
    public override bool UseItem()
    {
        if (Pickups.Instance.CurrentBits >= bitsToUpgrade)
        {
            Inventary.Instance.Player.upgradeStats.UpgradeHealth();
            bitsToUpgrade *= multiplier;
            Mathf.Round(bitsToUpgrade);
            return true;
        }
        return false;
    }

}