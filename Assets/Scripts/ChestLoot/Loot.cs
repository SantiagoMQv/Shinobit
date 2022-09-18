using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour, ISaveGame
{
    [Header("Loot")]
    [SerializeField] private DropItem[] availableLoot;
    

    private List<DropItem> selectedLoot = new List<DropItem>();
    public List<DropItem> SelectedLoot => selectedLoot;

    // Permite desde el editor generar un ID identificativo que permitirá gestionar el guardado de datos para este objeto
    [ContextMenu("Generar guid para ID")]
    private void GenerateGuid()
    {
        foreach (DropItem item in availableLoot)
        {
            item.id = System.Guid.NewGuid().ToString();
        }
    }

    private void Start()
    {
        SelectLoot();
    }

    private void SelectLoot()
    {
        foreach (DropItem item in availableLoot)
        {
            float chance = Random.Range(0, 100);
            if (chance <= item.dropChance)
            {
                selectedLoot.Add(item);
            }
        }
    }

    public void LoadData(GameData data)
    {
        foreach (DropItem loot in availableLoot)
        {
            data.pickedUpLoot.TryGetValue(loot.id, out loot.pickedUpItem);
        }
        
        
    }

    public void SaveData(ref GameData data)
    {
        
        foreach (DropItem loot in availableLoot)
        {
            if (data.pickedUpLoot.ContainsKey(loot.id))
            {
                data.pickedUpLoot.Remove(loot.id);
            }
            data.pickedUpLoot.Add(loot.id, loot.pickedUpItem);

        }
    }
}
