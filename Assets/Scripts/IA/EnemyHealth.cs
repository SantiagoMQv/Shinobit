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
        LootEnemy loot = GetComponent<LootEnemy>();
        Vector3 RandomizeIntensity = new Vector3(1f, 1f, 1f);
        foreach (DropEnemyItem item in loot.SelectedLoot)
        {
            Vector3 itemPosition = transform.position + new Vector3(Random.Range(-RandomizeIntensity.x, RandomizeIntensity.x),
                                                                    Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y),
                                                                    Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z));

            Instantiate(item.PrefabItem, itemPosition, Quaternion.identity);
        }
        // Se elimina de la escena así para que de tiempo a "salvar" los FloatingText que quedasen adheridos al enemigo.
        this.gameObject.SetActive(false);
        Destroy(this.gameObject, 5);
    }

}
