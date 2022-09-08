using UnityEngine;

[CreateAssetMenu(menuName ="Items/Weapon")]
public class WeaponItem : InventaryItem
{
    [Header("Weapon")]
    public Weapon Weapon;

    public override bool EquipWeaponContainer1()
    {
        if(WeaponContainer.Instance.EquippedWeapon1 == null && this != WeaponContainer.Instance.EquippedWeapon2)
        {
            WeaponContainer.Instance.EquipWeaponContainer1(this);
            return true;
        }
        return false;
    }

    public override bool EquipWeaponContainer2()
    {
        if (WeaponContainer.Instance.EquippedWeapon2 == null && this != WeaponContainer.Instance.EquippedWeapon1)
        {
            WeaponContainer.Instance.EquipWeaponContainer2(this);
            return true;
        }
        return false;
    }

    public override bool RemoveItem()
    {
        if (WeaponContainer.Instance.EquippedWeapon1 != null || WeaponContainer.Instance.EquippedWeapon2 != null)
        {
            WeaponContainer.Instance.RemoveWeapon(this);
            return true;
        }
        return false;
    }

}
