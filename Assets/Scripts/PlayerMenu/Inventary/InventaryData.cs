using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventaryData : MonoBehaviour
{
    [SerializeField] private InventaryItem[] allItems;

    private Inventary inventary;

    public InventaryItem[] AllItems => allItems;
    private void Start()
    {
        inventary = GetComponent<Inventary>();
    }
    public void loadInventaryData(GameData data)
    {
        
        if(data.itemData == null)
        {
            initializeItemData(ref data.itemData);
        }
        foreach (InventaryItem item in allItems)
        {
            for (int i = 0; i < data.itemData.Length; i++)
            {
                if (item.ID == data.itemData[i].ID)
                {
                    
                    if (item.Type == ItemType.UpgradeItem)
                    {
                        UpgradeItem upgradeItem = (UpgradeItem)item;
                        upgradeItem.bitsToUpgrade = data.itemData[i].BitsToUpgrade;
                        Inventary.Instance.AddItem(upgradeItem, data.itemData[i].Amount);
                    }
                    else
                    {
                        
                        Inventary.Instance.AddItem(item, data.itemData[i].Amount);
                    }                
                }
            }
        }
    }

    public void saveInventaryData(ref GameData data)
    {
        if (data.itemData == null)
        {
            initializeItemData(ref data.itemData);
        }
        int i = 0;
        foreach (InventaryItem item in inventary.InventaryItems)
        {
            if(item != null)
            {
                data.itemData[i].ID = item.ID;
                data.itemData[i].Amount = item.Amount;
                if (item.Type == ItemType.UpgradeItem)
                {
                    UpgradeItem upgradeItem = (UpgradeItem)item;
                    data.itemData[i].BitsToUpgrade = upgradeItem.bitsToUpgrade;
                }
                i++;
            }
        }
    }

    private void initializeItemData(ref ItemData[] itemData)
    {
        itemData = new ItemData[AllItems.Length - 1];
        for (int i = 0; i < itemData.Length; i++)
        {
            itemData[i] = new ItemData();
        }
    }
}
