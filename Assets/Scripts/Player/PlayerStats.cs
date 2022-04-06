using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Sirve para indicar a Unity que se puede crear en el menú
[CreateAssetMenu(menuName = "Stats")]
public class PlayerStats : ScriptableObject
{
    public float Damage = 1;
    public float Defense = 1;
    public float Speed = 1;
    [Range(0, 100)] public float CriticPercent;
    [Range(0, 100)] public float CriticBlock;

    public void ResetValues()
    {
        Damage = 1;
        Defense = 1;
        Speed = 1;
        CriticPercent = 0;
        CriticBlock = 0;
    }
}
