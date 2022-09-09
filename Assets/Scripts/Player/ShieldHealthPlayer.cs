using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHealthPlayer : HealthBase
{
    [SerializeField] private MiniHealthBar ShieldBarPrefab;
    [SerializeField] private Transform ShieldBarPosition;
    [SerializeField] private MiniHealthBar shieldHealthBar;

    public MiniHealthBar ShieldHealthBar => shieldHealthBar;

    protected override void Start()
    {
        base.Start();
    }


    protected override void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        shieldHealthBar.ModifyHealth(currentHealth, maxHealth);
    }

    // Aunque ponga "Defeated" se activa cuando la barra llega a 0.
    protected override void DefeatedCharacter()
    {
        shieldHealthBar.gameObject.transform.parent.gameObject.SetActive(false);
        this.enabled = false;
        Player.Instance.combatPlayer.ShieldNinjutsuVFX.SetActive(false);
    }

    public void setInitialMaxHealth(float health)
    {
        initialHealth = health;
        maxHealth = health;
    }

    private void OnEnable()
    {
        Health = initialHealth;
        UpdateHealthBar(Health, maxHealth);
        shieldHealthBar.gameObject.transform.parent.gameObject.SetActive(true);
    }

}
