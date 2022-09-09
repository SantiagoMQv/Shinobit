using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthBase
{
    [SerializeField] private MiniHealthBar HealthBarPrefab;
    [SerializeField] private Transform HealthBarPosition;

    private MiniHealthBar enemyHealthBar;


    private void Awake()
    {

    }

    protected override void Start()
    {
        base.Start();
        CreateHealthBar();
    }



    private void CreateHealthBar()
    {
        enemyHealthBar = Instantiate(HealthBarPrefab, HealthBarPosition);
        UpdateHealthBar(Health, maxHealth);
    }

    protected override void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        enemyHealthBar.ModifyHealth(currentHealth, maxHealth);
    }

    protected override void DefeatedCharacter()
    {
        // Se elimina de la escena así para que de tiempo a "salvar" los FloatingText que quedasen adheridos al enemigo.
        this.gameObject.SetActive(false);
        Destroy(this.gameObject, 5);
    }

}
