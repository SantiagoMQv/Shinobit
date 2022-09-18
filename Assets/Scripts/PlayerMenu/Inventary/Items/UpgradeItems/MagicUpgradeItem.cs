using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UpgradeItem/MagicUpgrade")]
public class MagicUpgradeItem : UpgradeItem
{
    public override bool UseItem()
    {
        if (Pickups.Instance.CurrentBits >= bitsToUpgrade)
        {
            Inventary.Instance.Player.upgradeStats.UpgradeSpell();
            bitsToUpgrade *= multiplier;
            Mathf.Round(bitsToUpgrade);
            return true;
        }
        return false;
    }

}
