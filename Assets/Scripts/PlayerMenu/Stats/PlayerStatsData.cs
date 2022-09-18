using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatsData 
{
    public int Damage;
    public int Defense;
    public int Potion;
    public int HealthPoints;
    public int SpellPoints;
    public int StaminaPoints;

    public PlayerStatsData()
    {
        this.Damage = 5;
        this.Defense = 1;
        this.Potion = 3;
        this.HealthPoints = 1;
        this.SpellPoints = 1;
        this.StaminaPoints = 1;
    }

}
