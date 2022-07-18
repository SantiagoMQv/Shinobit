using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventary : Singleton<Inventary>
{
    [SerializeField] private Player player;
    [SerializeField] private int slotNum;
    [SerializeField] public InventarySpecialItems specialItems;

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
    [HideInInspector] public HealingNinjutsuItem healingNinjutsuItem;

    public static Action PickupHealingNinjutsuItemEvent;

    private void Start()
    {
        inventaryItems = new InventaryItem[slotNum];
    }

    public void AddItem(InventaryItem itemToAdd, int amount)
    {
        if(itemToAdd == null)
        {
            return;
        }
        // En caso de existir slots con el item que se quiere añadir
        List<int> indexs = ExistCheck(itemToAdd.ID);
        if (itemToAdd.IsCumulative)
        {
            if(indexs.Count > 0)
            {
                for (int i = 0; i < indexs.Count; i++)
                {
                    if(inventaryItems[indexs[i]].Amount < itemToAdd.MaxAccumulation)
                    {
                        inventaryItems[indexs[i]].Amount += amount;
                        if (inventaryItems[indexs[i]].Amount > itemToAdd.MaxAccumulation)
                        {
                            int differenceAmount = inventaryItems[indexs[i]].Amount - itemToAdd.MaxAccumulation;
                            inventaryItems[indexs[i]].Amount = itemToAdd.MaxAccumulation;
                            AddItem(itemToAdd, differenceAmount);
                        }
                        InventaryUI.Instance.DrawnItemInInventary(itemToAdd, inventaryItems[indexs[i]].Amount, indexs[i]);
                        return;
                    }
                }
            }
        }

        // En caso de no existir slots con el item que se quiere añadir
        if(amount <= 0)
        {
            return;
        }

        if(amount > itemToAdd.MaxAccumulation)
        {
            AddItemInAvailableSlot(itemToAdd, itemToAdd.MaxAccumulation);
            amount -= itemToAdd.MaxAccumulation;
            AddItem(itemToAdd, amount);
        }
        else
        {
            AddItemInAvailableSlot(itemToAdd, amount);
            //Si es un objeto especial, no es acumulable y solo se obtendrá una vez, por lo que solo existe este caso.
            if(itemToAdd.IsSpecialItem)
            {
                if(itemToAdd.specialItem == SpecialItems.HealingNinjutsu)
                {
                    specialItems.HealingNinjutsu = true;
                    healingNinjutsuItem = (HealingNinjutsuItem) itemToAdd;
                    PickupHealingNinjutsuItemEvent?.Invoke();
                }
            }
        }
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
            inventaryItems[index] = null;
            InventaryUI.Instance.DrawnItemInInventary(null, 0, index);
        }
        else
        {
            InventaryUI.Instance.DrawnItemInInventary(inventaryItems[index], inventaryItems[index].Amount, index);
        }
    }

    private void UseItem(int index)
    {
        if(inventaryItems[index] != null)
        {
            if (inventaryItems[index].Type == ItemTypes.UpgradeItems)
            {
                if (inventaryItems[index].UseItem())
                {
                    UpgradeItem upgradeItem = ChooseUpgradeItem(Inventary.Instance.InventaryItems[index].upgradeItem);
                    RemoveItem(index);
                    Pickups.Instance.RemoveBits(upgradeItem.bitsToUpgrade);
                    upgradeItem.bitsToUpgrade = upgradeItem.bitsToUpgrade * upgradeItem.multiplier;
                    InventaryUI.Instance.UpdateInventaryDescription(index);
                    InventaryUI.Instance.UpdateButtons(index);
                }
            }
        }
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
                UseItem(index);
                break;
            case InteractionType.Remove:
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

    #endregion
}
