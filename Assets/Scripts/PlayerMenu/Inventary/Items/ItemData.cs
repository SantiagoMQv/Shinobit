using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string ID;
    public float BitsToUpgrade;
    public int Amount;

    public ItemData()
    {
        BitsToUpgrade = 100;
        Amount = 0;
    }
}
