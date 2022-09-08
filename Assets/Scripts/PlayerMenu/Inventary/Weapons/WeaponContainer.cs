using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponContainer : Singleton<WeaponContainer>
{
    [SerializeField] private Image weapon1Icon;
    [SerializeField] private TextMeshProUGUI letterGuide1TMP;
    [SerializeField] private Image weapon2Icon;
    [SerializeField] private TextMeshProUGUI letterGuide2TMP;
    public WeaponItem EquippedWeapon1 { get; set; }
    public WeaponItem EquippedWeapon2 { get; set; }

    

    public void EquipWeaponContainer1(WeaponItem weaponItem)
    {
        EquippedWeapon1 = weaponItem;
        weapon1Icon.sprite = weaponItem.Weapon.WeaponIcon;
        weapon1Icon.gameObject.SetActive(true);
        letterGuide1TMP.gameObject.SetActive(true);
        Player.Instance.combatPlayer.EquipWeapon1(weaponItem);
    }

    public void EquipWeaponContainer2(WeaponItem weaponItem)
    {
        EquippedWeapon2 = weaponItem;
        weapon2Icon.sprite = weaponItem.Weapon.WeaponIcon;
        weapon2Icon.gameObject.SetActive(true);
        letterGuide2TMP.gameObject.SetActive(true);
        Player.Instance.combatPlayer.EquipWeapon2(weaponItem);
    }

    public void RemoveWeapon(WeaponItem weaponItem)
    {
        if(weaponItem == EquippedWeapon1)
        {
            weapon1Icon.gameObject.SetActive(false);
            letterGuide1TMP.gameObject.SetActive(false);
            EquippedWeapon1 = null;
            Player.Instance.combatPlayer.RemoveWeapon(weaponItem);
        }
        else if (weaponItem == EquippedWeapon2)
        {
            weapon2Icon.gameObject.SetActive(false);
            letterGuide2TMP.gameObject.SetActive(false);
            EquippedWeapon2 = null;
            Player.Instance.combatPlayer.RemoveWeapon(weaponItem);
        }

    }

}
