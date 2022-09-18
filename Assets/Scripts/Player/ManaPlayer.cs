using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPlayer : MonoBehaviour
{
    [SerializeField] private float initialMana;
    [SerializeField] private float maxMana;
    [SerializeField] private float regenerationPerSecond;

    public float CurrentMana { get; private set; }

    private HealthPlayer healthPlayer;

    public float TotalMana => maxMana + ((Player.Instance.Stats.SpellPoints - 1) * 15);

    private void Awake()
    {
        healthPlayer = GetComponent<HealthPlayer>();
    }

    void Start()
    {
        CurrentMana = TotalMana;
        UpdateManaBar();

        //Este método se invoca las veces que yo quiera por segundo
        InvokeRepeating(nameof(ManaRegenerate), 1, 1);
    }

    public void UpgradeMana()
    {
        UpdateManaBar();
    }

    public void UseMana(float amount)
    {
        if(CurrentMana > amount)
        {
            CurrentMana -= amount;
            
            UpdateManaBar();

        }
    }

    private void ManaRegenerate()
    {
        if(healthPlayer.Health > 0 && CurrentMana < TotalMana)
        {
            CurrentMana += regenerationPerSecond;
            if (CurrentMana > TotalMana)
            {
                CurrentMana = TotalMana;
            }
            UpdateManaBar();
        }
    }

    public void ManaRestore()
    {
        CurrentMana = TotalMana;
        UpdateManaBar();
    }

    protected void UpdateManaBar()
    {
        UIManager.Instance.UpdatePlayerMana(CurrentMana, TotalMana);
    }
}
