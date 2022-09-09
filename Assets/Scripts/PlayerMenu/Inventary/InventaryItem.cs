using UnityEngine;

public enum ItemType
{
    Weapon,
    UpgradeItem
}

public enum SpecialItems
{
    None,
    HealingNinjutsu,
    SpearWeapon,
    ShurikenWeapon,
    ShieldNinjutsu
}

public enum UpgradeItems
{
    None,
    AttackUpgrade,
    DefenseUpgrade,
    PotionUpgrade,
    HealthUpgrade,
    MagicUpgrade,
    ResistenceUpgrade
}

public class InventaryItem : ScriptableObject
{
    [Header("Parameters")]
    public string ID;
    public string Name;
    public Sprite icon;
    [TextArea] public string Description;

    [Header("Information")]
    public ItemType Type;
    public bool IsEquippable;
    public bool IsCumulative;
    public UpgradeItems upgradeItem;
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

    public virtual bool EquipWeaponContainer1()
    {
        return true;
    }
    public virtual bool EquipWeaponContainer2()
    {
        return true;
    }
    public virtual bool RemoveItem()
    {
        return true;
    }
}
