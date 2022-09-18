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
    private PlayerJump playerJump;
    public bool CanBeRegenerate { get; private set; }
    public float TotalStamina => maxStamina + ((Player.Instance.Stats.StaminaPoints - 1) * 15);
    private void Awake()
    {
        healthPlayer = GetComponent<HealthPlayer>();
        playerJump = GetComponent<PlayerJump>();
    }
    void Start()
    {

        CurrentStamina = TotalStamina;
        CanBeRegenerate = true;
        UpdateStaminaBar();

        //Este método se invoca las veces que yo quiera por segundo
        InvokeRepeating(nameof(StaminaRegenerate), 1, 1);
    }
    public void UpgradeStamina()
    {
        UpdateStaminaBar();
    }

    public void UseStamina(float amount)
    {
        if (CurrentStamina > 0 && CanBeRegenerate)
        {
            if(CurrentStamina < amount)
            {
                CurrentStamina = 0;
                if (playerJump.Jumping)
                {
                    StartCoroutine("WaitForRegenerateWithJump");
                }
                else
                {
                    StartCoroutine("WaitForRegenerate");
                }
                
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
        CanBeRegenerate = false;
        playerJump.SetCanJump(false);
        yield return new WaitForSeconds(2);
        ResetColorBarStamina();
        CanBeRegenerate = true;
        playerJump.SetCanJump(true);
        StaminaRegenerate();
    }
    IEnumerator WaitForRegenerateWithJump()
    {
        UpdateColorBarStaminaToBlack();
        CanBeRegenerate = false;
        playerJump.SetCanJump(false);
        yield return new WaitForSeconds(3f);
        ResetColorBarStamina();
        CanBeRegenerate = true;
        playerJump.SetCanJump(true);
        StaminaRegenerate();
    }

    private void StaminaRegenerate()
    {
        if (healthPlayer.Health > 0 && CurrentStamina < TotalStamina && CanBeRegenerate)
        {
            CurrentStamina += regenerationPerSecond;
            if(CurrentStamina > TotalStamina)
            {
                CurrentStamina = TotalStamina;
            }
            UpdateStaminaBar();
        }
    }

    public void StaminaRestore()
    {
        CurrentStamina = TotalStamina;
        UpdateStaminaBar();
    }

    protected void UpdateStaminaBar()
    {
        UIManager.Instance.UpdatePlayerStamina(CurrentStamina, TotalStamina);
    }

    protected void UpdateColorBarStaminaToBlack()
    {
        UIManager.Instance.UpdateColorStaminaToBlack();
    }

    protected void ResetColorBarStamina()
    {
        UIManager.Instance.ResetColorStamina();
    }

    public void SetCanBeRegenerate(bool result)
    {

        CanBeRegenerate = result;
    }
}