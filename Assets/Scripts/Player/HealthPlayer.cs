using System;
using UnityEngine;

public class HealthPlayer : Healthbase
{
    public static Action DefeatedPlayerEvent;
    public bool CanBeHealed => Health < maxHealth;
    public bool Defeated { get; private set; }

    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
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
            GetDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestoreHealth(10);
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

    protected override void DefeatedPlayer()
    {   
        //Lo desactivamos al morir para que no hayan problemas de colisiones
        boxCollider2D.enabled = false;
        //Si no está vacío, se invoca
        DefeatedPlayerEvent?.Invoke();
        Defeated = true;
    }

    public void PlayerRestore()
    {
        boxCollider2D.enabled = true;
        Defeated = false;
        Health = initialHealth;
        UpdateHealthBar(Health, initialHealth); 
    }

    protected override void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        UIManager.Instance.UpdatePlayerHealth(currentHealth, maxHealth);
    }

}