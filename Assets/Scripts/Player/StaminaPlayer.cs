using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaPlayer : MonoBehaviour
{
    [SerializeField] private float initialStamina;
    [SerializeField] private float maxStamina;
    [SerializeField] private float regenerationPerSecond;

    public float CurrentStamina { get; private set; }

    private HealthPlayer healthPlayer;
    private bool CanBeRegenerate;
    private void Awake()
    {
        healthPlayer = GetComponent<HealthPlayer>();
    }
    void Start()
    {

        CurrentStamina = initialStamina;
        CanBeRegenerate = true;
        UpdateStaminaBar();

        //Este método se invoca las veces que yo quiera por segundo
        InvokeRepeating(nameof(StaminaRegenerate), 1, 1);
    }

    private void Update()
    {
        //Para hacer pruebas con el estamina 
        if (Input.GetKeyDown(KeyCode.H))
        {
            UseStamina(40);
        }
    }

    public void UseStamina(float amount)
    {
        if (CurrentStamina > 0 && CanBeRegenerate)
        {
            if(CurrentStamina < amount)
            {
                CurrentStamina = 0;
                CanBeRegenerate = false;
                StartCoroutine("WaitForRegenerate");
            }
            else
            {
                CurrentStamina -= amount;
            }
            
            UpdateStaminaBar();

        }
    }

    IEnumerator WaitForRegenerate()
    {
        UpdateColorBarStaminaToBlack();
        yield return new WaitForSeconds(2);
        ResetColorBarStamina();
        CanBeRegenerate = true;
        StaminaRegenerate();
    }

    private void StaminaRegenerate()
    {
        if (healthPlayer.Health > 0 && CurrentStamina < maxStamina && CanBeRegenerate)
        {
            CurrentStamina += regenerationPerSecond;
            UpdateStaminaBar();
        }
    }

    public void StaminaRestore()
    {
        CurrentStamina = initialStamina;
        UpdateStaminaBar();
    }

    protected void UpdateStaminaBar()
    {
        UIManager.Instance.UpdatePlayerStamina(CurrentStamina, maxStamina);
    }

    protected void UpdateColorBarStaminaToBlack()
    {
        UIManager.Instance.UpdateColorStaminaToBlack();
    }

    protected void ResetColorBarStamina()
    {
        UIManager.Instance.ResetColorStamina();
    }
}