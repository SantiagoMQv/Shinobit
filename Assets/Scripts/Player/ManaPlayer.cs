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


    private void Awake()
    {
        healthPlayer = GetComponent<HealthPlayer>();
    }

    void Start()
    {
        CurrentMana = initialMana;
        UpdateManaBar();

        //Este método se invoca las veces que yo quiera por segundo
        InvokeRepeating(nameof(ManaRegenerate), 1, 1);
    }

    private void Update()
    {
        //Para hacer pruebas con el mana 
        if (Input.GetKeyDown(KeyCode.G))
        {
            UseMana(10);
        }
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
        if(healthPlayer.Health > 0 && CurrentMana < maxMana)
        {
            CurrentMana += regenerationPerSecond;
            UpdateManaBar();
        }
    }

    public void ManaRestore()
    {
        CurrentMana = initialMana;
        UpdateManaBar();
    }

    protected void UpdateManaBar()
    {
        UIManager.Instance.UpdatePlayerMana(CurrentMana, maxMana);
    }
}
