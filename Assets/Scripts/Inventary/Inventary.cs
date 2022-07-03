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

    public Player Player => player;
    public InventaryItem[] InventaryItems => inventaryItems;
    public int SlotNum => slotNum;
    [HideInInspector] public HealingNinjutsu healingNinjutsuItem;
    public int CurrentBits { get; set; }

    public static Action PickupHealingNinjutsuItemEvent;

    private void Start()
    {
        inventaryItems = new InventaryItem[slotNum];
        CurrentBits = 0;
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
                    healingNinjutsuItem = (HealingNinjutsu) itemToAdd;
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

    #region Bits

    public void AddBits(int amount)
    {
        CurrentBits += amount;
    }

    public void RemoveBits(int amount)
    {
        CurrentBits -= amount;
    }

    #endregion

}
