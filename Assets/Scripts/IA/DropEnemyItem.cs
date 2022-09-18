using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DropEnemyItem
{
    [Header("Info")]
    public GameObject PrefabItem;


    [Header("Drop")]
    [Range(0, 100)] public float dropChance;

}
