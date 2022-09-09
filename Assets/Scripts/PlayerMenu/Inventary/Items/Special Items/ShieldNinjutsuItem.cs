using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/ShieldNinjutsu")]
public class ShieldNinjutsuItem : InventaryItem
{
    [Header("General info")]
    public float InitialShieldHP;
    public float reuseTime;
    public PlayerStats stats;

    public override bool UseItem()
    {
        ShieldHealthPlayer shieldHealth = Player.Instance.gameObject.GetComponent<ShieldHealthPlayer>();
        shieldHealth.setInitialMaxHealth(getHealthFromStats());
        shieldHealth.enabled = true;
        return true;
    }

    private float getHealthFromStats()
    {
        return InitialShieldHP + (stats.Defense * 3);
    }
}
