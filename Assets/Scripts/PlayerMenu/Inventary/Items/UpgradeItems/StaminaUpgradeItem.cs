using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UpgradeItem/StaminaUpgrade")]
public class StaminaUpgradeItem : UpgradeItem
{
    public override bool UseItem()
    {
        if (Pickups.Instance.CurrentBits >= bitsToUpgrade)
        {
            Inventary.Instance.Player.upgradeStats.UpgradeStamina();
            bitsToUpgrade *= multiplier;
            Mathf.Round(bitsToUpgrade);
            return true;
        }
        return false;
    }
}

    