using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Sirve para indicar a Unity que se puede crear en el menú
[CreateAssetMenu(menuName = "Stats")]
public class PlayerStats : ScriptableObject
{
    public float Damage = 1;
    public float Defense = 1;
    public float Potion = 1;
    public float HealthPoints = 1;
    public float SpellPoints = 1;
    public float StaminaPoints = 1;

    public void ResetValues()
    {
        Damage = 1;
        Defense = 1;
        Potion = 1;
        HealthPoints = 1;
        SpellPoints = 1;
        StaminaPoints = 1;
    }
}
