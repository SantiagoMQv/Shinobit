using System;
using UnityEngine;

public class HealthPlayer : HealthBase
{

    public static Action DefeatedPlayerEvent;
    public bool CanBeHealed => Health < maxHealth;
    public bool Defeated { get; private set; }

    private BoxCollider2D boxCollider2D;
    private PlayerJump playerJump;

    public float TotalHealth => maxHealth + ((Player.Instance.Stats.HealthPoints - 1) * 15);

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        playerJump = GetComponent<PlayerJump>();
        Defeated = false;
    }

    protected override void Start()
    {
        base.Start();
        Health = TotalHealth;
        maxHealth = TotalHealth;
        //Debug.Log(maxHealth);
        UpdateHealthBar(Health, maxHealth);
    }

    private void Update()
    {
        if (Defeated)
        {
            Player.Instance.movementPlayer.SetCanMove(false);
        }
    }

    public void UpgradeHealth()
    {
        UpdateHealthBar(Health, TotalHealth);
    }

    public void RestoreHealth(float amount)
    {
        if (Defeated)
        {
            return;
        }
        if (CanBeHealed)
        {
            Health += amount;
            if(Health > maxHealth)
            {
                Health = maxHealth;
            }

            UpdateHealthBar(Health, maxHealth);
        }
    }

    public override void GetDamage(float amount, GameObject enemy)
    {
        if (!playerJump.Jumping)
        {

            if (GetComponent<ShieldHealthPlayer>().enabled)
            {
                GetComponent<ShieldHealthPlayer>().GetDamage(amount, enemy);
            }
            else
            {
                base.GetDamage(amount, enemy);
            }
        }
        
    }

    protected override void DefeatedCharacter()
    {
        //Lo desactivamos al morir para que no hayan problemas de colisiones
        //boxCollider2D.enabled = false;
        
        DeathManager.Instance.OpenDeathPanel();
        DefeatedPlayerEvent?.Invoke();
        Defeated = true;

    }

    public void PlayerRestore()
    {
        //boxCollider2D.enabled = true;
        Defeated = false;
        Health = maxHealth;
        UpdateHealthBar(Health, maxHealth); 
    }

    protected override void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        UIManager.Instance.UpdatePlayerHealth(currentHealth, maxHealth);
    }

}