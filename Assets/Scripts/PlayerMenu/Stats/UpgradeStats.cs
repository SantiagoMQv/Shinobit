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
        Player.Instance.HealthPlayer.UpgradeHealth();
    }
    public void UpgradePotion()
    {
        playerStats.Potion++;
        Player.Instance.combatPlayer.AddAllHealthToken();
    }
    public void UpgradeSpell()
    {
        playerStats.SpellPoints++;
        Player.Instance.ManaPlayer.UpgradeMana();
    }
    public void UpgradeStamina()
    {
        playerStats.StaminaPoints++;
        Player.Instance.StaminaPlayer.UpgradeStamina();
    }
}
