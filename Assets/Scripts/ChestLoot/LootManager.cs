using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : Singleton<LootManager>
{
    [SerializeField] private GameObject lootPanel;
    [SerializeField] private LootButton lootButtonPrebab;
    [SerializeField] private Transform lootContainer;
    public void ShowLootPanel(Loot loot)
    {
        lootPanel.SetActive(true);
        if (BusyContainer())
        {
            foreach (Transform child in lootContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }

        for (int i = 0; i < loot.SelectedLoot.Count; i++)
        {
            LoadLootPanel(loot.SelectedLoot[i]);
        }

    }

    public void CloseLootPanel()
    {
        lootPanel.SetActive(false);
    }

    private void LoadLootPanel(DropItem dropItem)
    {
        if (!dropItem.pickedUpItem)
        {
            LootButton loot = Instantiate(lootButtonPrebab, lootContainer);
            loot.SetUpLootItem(dropItem);
            loot.transform.SetParent(lootContainer);
        }
    }

    private bool BusyContainer()
    {
        LootButton[] childen = lootContainer.GetComponentsInChildren<LootButton>();
        if (childen.Length > 0)
        {
            return true;
        }
        return false;
    }


}
