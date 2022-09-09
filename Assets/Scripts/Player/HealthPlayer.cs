using System;
using UnityEngine;

public class HealthPlayer : HealthBase
{
    public static Action DefeatedPlayerEvent;
    public bool CanBeHealed => Health < maxHealth;
    public bool Defeated { get; private set; }

    private BoxCollider2D boxCollider2D;
    private PlayerJump playerJump;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        playerJump = GetComponent<PlayerJump>();
    }

    protected override void Start()
    {
        base.Start();
        UpdateHealthBar(Health, maxHealth);
    }

    private void Update()
    {
        //Para hacer pruebas con el daño recibido
        if (Input.GetKeyDown(KeyCode.T))
        {
            GetDamage((float) Math.Round(10.8), null);
        }
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
        //Si no está vacío, se invoca
        DefeatedPlayerEvent?.Invoke();
        Defeated = true;
    }

    public void PlayerRestore()
    {
        //boxCollider2D.enabled = true;
        Defeated = false;
        Health = initialHealth;
        UpdateHealthBar(Health, initialHealth); 
    }

    protected override void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        UIManager.Instance.UpdatePlayerHealth(currentHealth, maxHealth);
    }

}