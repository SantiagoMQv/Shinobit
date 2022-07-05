using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UpgradeItem/StaminaUpgrade")]
public class StaminaUpgradeItem : UpgradeItem
{
    public override bool UseItem()
    {
        if (Inventary.Instance.CurrentBits >= bitsToUpgrade)
        {
            Inventary.Instance.Player.PlayerStats.StaminaPoints++;
            return true;
        }
        return false;
    }
}

    