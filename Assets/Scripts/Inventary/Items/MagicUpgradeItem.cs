using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UpgradeItem/MagicUpgrade")]
public class MagicUpgradeItem : UpgradeItem
{
    public override bool UseItem()
    {
        if (Inventary.Instance.CurrentBits >= bitsToUpgrade)
        {
            Inventary.Instance.Player.PlayerStats.SpellPoints++;
            bitsToUpgrade *= multiplier;
            return true;
        }
        return false;
    }

}
