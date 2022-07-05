using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum InteractionType
{
    Click,
    UseEquip,
    Remove
}

public class InventarySlot : MonoBehaviour
{
    public static Action<InteractionType, int> SlotInteractionEvent;
    [SerializeField] private Image itemIcon;
    [SerializeField] private GameObject backgroundAmount;
    [SerializeField] private TextMeshProUGUI amountTMP;
    public int Index { get; set; }

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

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

    public void SelectSlot()
    {
        button.Select();
    }

    public void ClickSlot()
    {
        SlotInteractionEvent?.Invoke(InteractionType.Click, Index); // el ? es para invocarlo solo si no es nulo
    }

    public void UseEquipItemSlot()
    {
        if(Inventary.Instance.InventaryItems[Index] != null)
        {
            SlotInteractionEvent?.Invoke(InteractionType.UseEquip, Index);
        }
    }
}
