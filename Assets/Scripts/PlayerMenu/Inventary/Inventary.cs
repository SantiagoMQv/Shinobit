using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventary : Singleton<Inventary>, ISaveGame
{
    [SerializeField] private Player player;
    [SerializeField] private int slotNum;

    [Header("Items")]
    [SerializeField] private InventaryItem[] inventaryItems;

    [Header("Upgrade Items")]
    [SerializeField] private StaminaUpgradeItem staminaUpgradeItem;
    [SerializeField] private AttackUpgradeItem attackUpgradeItem;
    [SerializeField] private DefenseUpgradeItem defenseUpgradeItem;
    [SerializeField] private HealthUpgradeItem healthUpgradeItem;
    [SerializeField] private MagicUpgradeItem magicUpgradeItem;
    [SerializeField] private HealingUpgradeItem potionUpgradeItem;

    public Player Player => player;
    public InventaryItem[] InventaryItems => inventaryItems;
    public int SlotNum => slotNum;

    private InventaryData inventaryData;
    protected override void Awake()
    {
        base.Awake();
        inventaryItems = new InventaryItem[slotNum];
        inventaryData = GetComponent<InventaryData>();
    }

    public void InitializeInventaryItems()
    {
        inventaryItems = new InventaryItem[slotNum];
    }

    public void AddItem(InventaryItem itemToAdd, int amount)
    {
        if (itemToAdd == null)
        {
            return;
        }

        if (itemToAdd.IsCumulative)
        {
            AddCumulativeItem(itemToAdd, amount);
        }
        else
        {
            AddNonCumulativeItem(itemToAdd, amount);
        }
    }

    private void AddCumulativeItem(InventaryItem itemToAdd, int amount)
    {
        List<int> indexs = ExistCheck(itemToAdd.ID);
        if (indexs.Count > 0)
        {
            for (int i = 0; i < indexs.Count; i++)
            {
                if (inventaryItems[indexs[i]].Amount < itemToAdd.MaxAccumulation)
                {
                    inventaryItems[indexs[i]].Amount += amount;
                    if (inventaryItems[indexs[i]].Amount > itemToAdd.MaxAccumulation)
                    {
                        int differenceAmount = inventaryItems[indexs[i]].Amount - itemToAdd.MaxAccumulation;
                        inventaryItems[indexs[i]].Amount = itemToAdd.MaxAccumulation;
                        AddItem(itemToAdd, differenceAmount);
                    }
                    UpdateInventaryUI(itemToAdd, inventaryItems[indexs[i]].Amount, indexs[i]);
                    return;
                }
            }
        }

        if (amount <= 0 && itemToAdd.Type != ItemType.UpgradeItem)
        {
            return;
        }

        if (amount > itemToAdd.MaxAccumulation)
        {
            AddItemInAvailableSlot(itemToAdd, itemToAdd.MaxAccumulation);
            amount -= itemToAdd.MaxAccumulation;
            AddItem(itemToAdd, amount);
        }
        else
        {
            AddItemInAvailableSlot(itemToAdd, amount);
        }
    }

    private void AddNonCumulativeItem(InventaryItem itemToAdd, int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        if (amount > 1)
        {
            amount = 1;
        }

        AddItemInAvailableSlot(itemToAdd, amount);
        HandleSpecialItem(itemToAdd);
    }

    private void HandleSpecialItem(InventaryItem itemToAdd)
    {
        switch (itemToAdd.specialItem)
        {
            case SpecialItems.HealingNinjutsu:
                HealingNinjutsuItem healingNinjutsuItem = (HealingNinjutsuItem)itemToAdd;
                Player.Instance.combatPlayer.EquipHealingNinjutsu(healingNinjutsuItem);
                UIManager.Instance.AddHealingNinjutsuToGrid();
                break;
            case SpecialItems.SpearWeapon:
                WeaponItem SpearWeaponItem = (WeaponItem)itemToAdd;
                Player.Instance.combatPlayer.EquipSpearWeapon(SpearWeaponItem);
                UIManager.Instance.AddSpearToGrid();
                break;
            case SpecialItems.ShurikenWeapon:
                WeaponItem ShurikenWeaponItem = (WeaponItem)itemToAdd;
                Player.Instance.combatPlayer.EquipShurikenWeapon(ShurikenWeaponItem);
                UIManager.Instance.AddShurikenToGrid();
                break;
            case SpecialItems.ShieldNinjutsu:
                ShieldNinjutsuItem shield = (ShieldNinjutsuItem)itemToAdd;
                Player.Instance.combatPlayer.EquipShieldNinjutsu(shield);
                UIManager.Instance.AddShieldNinjutsuToGrid();
                break;
        }
    }

    private void UpdateInventaryUI(InventaryItem itemToAdd, int amount, int index)
    {
        InventaryUI.Instance.DrawnItemInInventary(itemToAdd, amount, index);
    }


    //Devuelve la lista de los index del inventario donde hay un objeto con el mismo 'itemID'
    private List<int> ExistCheck(string itemID)
    {
        List<int> indexsItem = new List<int>();
        for (int i = 0; i < inventaryItems.Length; i++)
        {
            if(inventaryItems[i] != null)
            {
                if (itemID == inventaryItems[i].ID)
                {
                    indexsItem.Add(i);
                }
            }
        }
        return indexsItem;
    }

    private void AddItemInAvailableSlot(InventaryItem itemToAdd, int amount)
    {
        
        for (int i = 0; i < inventaryItems.Length; i++)
        {
            if (inventaryItems[i] == null)
            {
                
                inventaryItems[i] = itemToAdd.CopyItem();
                inventaryItems[i].Amount = amount;
                InventaryUI.Instance.DrawnItemInInventary(itemToAdd, amount, i);
                return;
            }
        }
    }

    private void RemoveItem(int index)
    {
        inventaryItems[index].Amount--;
        if(inventaryItems[index].Amount <= 0)
        {
            inventaryItems[index].Amount = 0;
            InventaryUI.Instance.DrawnItemInInventary(inventaryItems[index], 0, index);
        }
        else
        {
            InventaryUI.Instance.DrawnItemInInventary(inventaryItems[index], inventaryItems[index].Amount, index);
        }
    }

    private void RemoveEquippedItem(int index)
    {

        if (inventaryItems[index] != null && inventaryItems[index].Type == ItemType.Weapon)
        {
            inventaryItems[index].RemoveItem();
        }
    }

    private void UseEquipItem(int index)
    {
        if(inventaryItems[index] != null)
        {
            if (inventaryItems[index].Type == ItemType.UpgradeItem)
            {
                if (inventaryItems[index].UseItem())
                {
                    UpgradeItem upgradeItem = ChooseUpgradeItem(Inventary.Instance.InventaryItems[index].upgradeItem);
                    RemoveItem(index);
                    Pickups.Instance.RemoveBits(upgradeItem.bitsToUpgrade);
                    upgradeItem.bitsToUpgrade = upgradeItem.bitsToUpgrade * upgradeItem.multiplier;
                    InventaryUI.Instance.UpdateInventaryDescription(index);
                    InventaryUI.Instance.UpdateButtons(index);
                    SaveManager.Instance.SaveGame();
                }
            }else if (inventaryItems[index].Type == ItemType.Weapon) // En este caso no se "usar�", sino que se "equipar�"
            {
                InventaryUI.Instance.OpenCloseWhereEquipPanel();
            }
        }
    }

    public void EquipWeaponContainer1(int index)
    {
        inventaryItems[index].EquipWeaponContainer1();
    }
    public void EquipWeaponContainer2(int index)
    {
        inventaryItems[index].EquipWeaponContainer2();
    }

    private UpgradeItem ChooseUpgradeItem(UpgradeItems type)
    {
        UpgradeItem item = null;
        switch (type)
        {
            case UpgradeItems.AttackUpgrade:
                item = attackUpgradeItem;
                break;
            case UpgradeItems.DefenseUpgrade:
                item = defenseUpgradeItem;
                break;
            case UpgradeItems.HealthUpgrade:
                item = healthUpgradeItem;
                break;
            case UpgradeItems.MagicUpgrade:
                item = magicUpgradeItem;
                break;
            case UpgradeItems.PotionUpgrade:
                item = potionUpgradeItem;
                break;
            case UpgradeItems.ResistenceUpgrade:
                item = staminaUpgradeItem;
                break;
        }
        return item;
    }

    #region Events

    private void SlotInteractionResponse(InteractionType type, int index)
    {
        switch (type)
        {
            case InteractionType.UseEquip:
                UseEquipItem(index);
                break;
            case InteractionType.Remove:
                RemoveEquippedItem(index);
                break;
        }
    }

    private void OnEnable()
    {
        InventarySlot.SlotInteractionEvent += SlotInteractionResponse;
    }


    private void OnDisable()
    {
        InventarySlot.SlotInteractionEvent -= SlotInteractionResponse;
    }

    public void LoadData(GameData data)
    {
        inventaryData.loadInventaryData(data);
    }

    public void SaveData(ref GameData data)
    {
        inventaryData.saveInventaryData(ref data);
    }

    #endregion
}
