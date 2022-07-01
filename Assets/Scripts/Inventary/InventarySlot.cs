using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum InteractionType
{
    Click,
    Use,
    Equip,
    Remove
}

public class InventarySlot : MonoBehaviour
{
    public static Action<InteractionType, int> SlotInteractionEvent;
    [SerializeField] private Image itemIcon;
    [SerializeField] private GameObject backgroundAmount;
    [SerializeField] private TextMeshProUGUI amountTMP;
    public int Index { get; set; }
    
    public void UpdateSlot(InventaryItem item, int amount)
    {
        itemIcon.sprite = item.icon;
        amountTMP.text = amount.ToString();
    }


    public void ActivateItemIcon(bool state)
    {
        itemIcon.gameObject.SetActive(state);
    }

    public void ActivateBackgroundAmount(bool state)
    {
        backgroundAmount.SetActive(state);
    }

    public void ClickSlot()
    {
        SlotInteractionEvent?.Invoke(InteractionType.Click, Index); // el ? es para invocarlo solo si no es nulo
    }
}
