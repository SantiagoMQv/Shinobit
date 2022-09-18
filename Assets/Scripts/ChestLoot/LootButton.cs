using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LootButton : MonoBehaviour
{
    [SerializeField] private Image ItemIcon;
    [SerializeField] private TextMeshProUGUI ItemNameTMP;
    [SerializeField] private Sprite BitIcon;


    public DropItem ItemToPickUp { get; set; }

    public void SetUpLootItem(DropItem dropItem)
    {
        if (dropItem.Item != null)
        {
            ItemToPickUp = dropItem;
            ItemIcon.sprite = dropItem.Item.icon;
            ItemNameTMP.text = $"{dropItem.Item.Name} x {dropItem.Amount}";
        }
        else
        {
            ItemToPickUp = dropItem;
            ItemIcon.sprite = BitIcon;
            ItemIcon.rectTransform.sizeDelta = new Vector2(50,75);
            //ItemIcon.rectTransform.localPosition = Vector3.zero;
            ItemIcon.rectTransform.localPosition = new Vector3(-200, 0, 0);
            ItemNameTMP.text = $"BITS x {dropItem.Amount}";
        }
        
    }

    public void GetItem()
    {
        if(ItemToPickUp != null)
        {
            if(ItemToPickUp.Item != null)
            {
                Inventary.Instance.AddItem(ItemToPickUp.Item, ItemToPickUp.Amount);
            }
            else
            {
                Pickups.Instance.AddBits(ItemToPickUp.Amount);
            }
            ItemToPickUp.pickedUpItem = true;
            Destroy(gameObject);

        }
    }
}
