using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeItem : InventaryItem
{
    public float bitsToUpgrade;
    public float multiplier;

    public void ResetValues()
    {
        bitsToUpgrade = 100;
    }
}
