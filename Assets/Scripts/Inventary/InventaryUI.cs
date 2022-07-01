using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventaryUI : Singleton<InventaryUI>
{
    [Header("Panel de descripci�n")]
    [SerializeField] private GameObject inventaryDescriptionPanel;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemNameTMP;
    [SerializeField] private TextMeshProUGUI itemDescriptionTMP;
    [Header("Principal")]
    [SerializeField] private InventarySlot slotPrefab;
    [SerializeField] private Transform container;

    public InventarySlot selectedSlot { get; private set; }
    List<InventarySlot> availableSlot = new List<InventarySlot>();

    void Start()
    {
        InitializeInventary();
    }

    private void Update()
    {
        UpdateSelectedSlot();
    }

    private void InitializeInventary()
    {
        InventarySlot newSlot;
        for (int i = 0; i < Inventary.Instance.SlotNum; i++)
        {
            newSlot = Instantiate(slotPrefab, container); // Instancia un objeto slotPrefab en container
            newSlot.Index = i;
            availableSlot.Add(newSlot);
        }
    }
    
    private void UpdateSelectedSlot()
    {
        GameObject gameObejectSelected = EventSystem.current.currentSelectedGameObject; // Devuelve el objeto que est� siendo actualmente seleccionado
        if(gameObejectSelected == null)
        {
            return;
        }

        InventarySlot slot = gameObejectSelected.GetComponent<InventarySlot>();
        if(slot != null)
        {
            selectedSlot = slot;
        }
    }

    public void DrawnItemInInventary(InventaryItem itemToAdd, int amount, int itemIndex)
    {
        InventarySlot slot = availableSlot[itemIndex];
        if (itemToAdd != null)
        {
            slot.ActivateItemIcon(true);
            if (amount > 1)
            {
                slot.ActivateBackgroundAmount(true);
            }
            slot.UpdateSlot(itemToAdd, amount);
        }
        else
        {
            slot.ActivateItemIcon(false);
            slot.ActivateBackgroundAmount(false);
        }
        
    }

    private void UpdateInventaryDescription(int index)
    {
        if(Inventary.Instance.InventaryItems[index] != null)
        {
            itemIcon.sprite = Inventary.Instance.InventaryItems[index].icon;
            itemNameTMP.text = Inventary.Instance.InventaryItems[index].Name;
            itemDescriptionTMP.text = Inventary.Instance.InventaryItems[index].Description;
            inventaryDescriptionPanel.SetActive(true);
        }
        else
        {
            inventaryDescriptionPanel.SetActive(false);
        }
    }

    private void SlotInteractionResponse(InteractionType type, int index)
    {
        if(type == InteractionType.Click)
        {
            UpdateInventaryDescription(index);
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
}
