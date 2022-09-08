using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBase : MonoBehaviour
{
    [SerializeField] protected float initialHealth;
    [SerializeField] protected float maxHealth;

    public static System.Action<string, Color, GameObject> EventFloatingText;
    public float Health { get; protected set; }

    protected virtual void Start()
    {
        Health = initialHealth;
    }

    
    public virtual void GetDamage(float amount, GameObject enemyDealingDamage)
    {

        if(amount <= 0)
        {
            return;
        }
        if(Health > 0)
        {
            Health -= amount;
            EventFloatingText?.Invoke(amount.ToString(), Color.red, enemyDealingDamage);
            UpdateHealthBar(Health, maxHealth);
            if (Health <= 0)
            {
                Health = 0;
                UpdateHealthBar(Health, maxHealth);
                DefeatedCharacter();
            }
        }

    }

    protected virtual void UpdateHealthBar(float currentHealth, float maxHealth)
    {

    }

    protected virtual void DefeatedCharacter()
    {

    }

}
