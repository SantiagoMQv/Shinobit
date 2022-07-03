using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UpgradeItem/PotionUpgrade")]
public class PotionUpgradeItem : UpgradeItem
{
    public override bool UseItem()
    {
        if(Inventary.Instance.CurrentBits >= bitsToUpgrade)
        {
            return true;
        }
        return false;
    }
}
