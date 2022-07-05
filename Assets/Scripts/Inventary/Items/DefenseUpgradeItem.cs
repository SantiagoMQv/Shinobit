using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UpgradeItem/DefenseUpgrade")]
public class DefenseUpgradeItem : UpgradeItem
{
    public override bool UseItem()
    {
        if (Inventary.Instance.CurrentBits >= bitsToUpgrade)
        {
            Inventary.Instance.Player.PlayerStats.Defense++;
            bitsToUpgrade *= multiplier;
            return true;
        }
        return false;
    }
}
