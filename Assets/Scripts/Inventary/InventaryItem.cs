using UnityEngine;

public enum ItemTypes
{
    Weapons,
    Ninjutsus,
    UpgradeItems
}

public enum SpecialItems
{
    None,
    HealingNinjutsu
}

public class InventaryItem : ScriptableObject
{
    [Header("Parameters")]
    public string ID;
    public string Name;
    public Sprite icon;
    [TextArea] public string Description;

    [Header("Information")]
    public ItemTypes Type;
    public bool IsEquippable;
    public bool IsCumulative;
    public bool IsConsumable;
    public bool IsSpecialItem;
    public SpecialItems specialItem;
    public int MaxAccumulation;

    [HideInInspector] public int Amount; // HideInInspector lo oculta del inspector


    // Necesario para que al añadir items al inventario, se añadan clones y no el ScriptableObject original
    public InventaryItem CopyItem()
    {
        InventaryItem newInstance = Instantiate(this);
        return newInstance;
    }

    public virtual bool UseItem()
    {
        return true;
    }

    public virtual bool EquipItem()
    {
        return true;
    }
    public virtual bool RemoveItem()
    {
        return true;
    }
}
