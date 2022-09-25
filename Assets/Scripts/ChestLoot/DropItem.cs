using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DropItem
{
    public string id;

    [Header("Info")]
    public string Name;
    public int Amount;
    public InventaryItem Item;
    [HideInInspector] public bool pickedUpItem;

    [Header("Drop")]
    [Range(0, 100)] public float dropChance;

    public bool PickedUpItem => pickedUpItem;
}
