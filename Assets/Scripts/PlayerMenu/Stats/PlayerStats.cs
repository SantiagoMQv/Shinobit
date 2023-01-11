using UnityEngine;


//Sirve para indicar a Unity que se puede crear en el men�
[CreateAssetMenu(menuName = "Stats")]
public class PlayerStats : ScriptableObject
{
    public int Damage = 1;
    public int Defense = 1;
    public int Potion = 1;
    public int HealthPoints = 1;
    public int SpellPoints = 1;
    public int StaminaPoints = 1;

    public void AddBonusForWeapon(Weapon weapon)
    {
        Damage += weapon.attack;
        Defense += weapon.defense;
    }

    public void RemoveBonusForWeapon(Weapon weapon)
    {
        Damage -= weapon.attack;
        Defense -= weapon.defense;
    }

    public void ResetValues()
    {
        Damage = 5;
        Defense = 1;
        Potion = 3;
        HealthPoints = 1;
        SpellPoints = 1;
        StaminaPoints = 1;
    }
}
