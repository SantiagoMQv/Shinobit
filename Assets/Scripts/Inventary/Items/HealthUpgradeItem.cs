using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UpgradeItem/HealthUpgrade")]
public class HealthUpgradeItem : UpgradeItem
{
    public override bool UseItem()
    {
        if (Inventary.Instance.CurrentBits >= bitsToUpgrade)
        {
            Inventary.Instance.Player.PlayerStats.HealthPoints++;
            bitsToUpgrade *= multiplier;
            return true;
        }
        return false;
    }

}