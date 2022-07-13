using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Ninjutsu/HealingNinjutsu")]
public class HealingNinjutsu : InventaryItem
{
    [Header("General info")]
    public float InitialHPRestoration;
    public int InitialTokens;
    [HideInInspector] public int currentNumTokens;
    [HideInInspector] public float currentHPRestoration;


}
