using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbase : MonoBehaviour
{
    [SerializeField] protected float initialHealth;
    [SerializeField] protected float maxHealth;

    public static System.Action<string, Color> EventFloatingText;
    public float Health { get; protected set; }

    protected virtual void Start()
    {
        Health = initialHealth;
    }

    
    public virtual void GetDamage(float amount)
    {

        if(amount <= 0)
        {
            return;
        }
        if(Health > 0)
        {
            Health -= amount;
            EventFloatingText?.Invoke(amount.ToString(), Color.red);
            UpdateHealthBar(Health, maxHealth);
            if (Health <= 0)
            {
                Health = 0;
                UpdateHealthBar(Health, maxHealth);
                DefeatedPlayer();
            }
        }

    }

    protected virtual void UpdateHealthBar(float currentHealth, float maxHealth)
    {

    }

    protected virtual void DefeatedPlayer()
    {

    }

}
