using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootEnemy : MonoBehaviour
{
    [Header("Loot")]
    [SerializeField] private DropEnemyItem[] availableLoot;

    private List<DropEnemyItem> selectedLoot = new List<DropEnemyItem>();
    public List<DropEnemyItem> SelectedLoot => selectedLoot;

    private void Start()
    {
        SelectLoot();
    }

    private void SelectLoot()
    {
        foreach (DropEnemyItem item in availableLoot)
        {
            float chance = Random.Range(0, 100);
            if (chance <= item.dropChance)
            {
                selectedLoot.Add(item);
            }
        }
    }
}
