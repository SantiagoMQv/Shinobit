using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniHealthBar : MonoBehaviour
{
    [SerializeField] private Image HealthBar;

    private float currentHealth;
    private float maxHealth;

    private void Update()
    {
        HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, currentHealth/maxHealth, 10f * Time.deltaTime);
    }

    public void ModifyHealth(float currentHealth, float maxHealth) {
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
    }
}
