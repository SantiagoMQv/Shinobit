using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStats : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    public PlayerStats PlayerStats => playerStats;

    public void UpgradeDamage()
    {
        playerStats.Damage++;
    }
    public void UpgradeDefense()
    {
        playerStats.Defense++;
    }
    public void UpgradeHealth()
    {
        playerStats.HealthPoints++;
    }
    public void UpgradePotion()
    {
        playerStats.Potion++;
    }
    public void UpgradeSpell()
    {
        playerStats.SpellPoints++;
    }
    public void UpgradeStamina()
    {
        playerStats.StaminaPoints++;
    }
}
