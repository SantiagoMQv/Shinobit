using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventaryUI : Singleton<InventaryUI>
{
    [Header("Panel de descripción")]
    [SerializeField] private GameObject inventaryDescriptionPanel;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemNameTMP;
    [SerializeField] private TextMeshProUGUI itemDescriptionTMP;
    [SerializeField] private TextMeshProUGUI buttonEquipUseTMP;
    [SerializeField] private GameObject buttonEquipUse;
    [SerializeField] private GameObject buttonRemove;
    [Header("Principal")]
    [SerializeField] private InventarySlot slotPrefab;
    [SerializeField] private Transform container;
    [SerializeField] private Player player;

    public InventarySlot selectedSlot { get; private set; }
    List<InventarySlot> availableSlot = new List<InventarySlot>();

    private PlayerNearToSaveAltar saveAltar;

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
        GameObject gameObejectSelected = EventSystem.current.currentSelectedGameObject; // Devuelve el objeto que está siendo actualmente seleccionado
        if(gameObejectSelected == null)
        {
            return;
        }

        InventarySlot slot = gameObejectSelected.GetComponent<InventarySlot>();
        if(slot != null)
        {
            selectedSlot = slot;

            if(Inventary.Instance.InventaryItems[selectedSlot.Index] != null)
            {
                // Botones
                if (Inventary.Instance.InventaryItems[selectedSlot.Index].Type == ItemTypes.UpgradeItems && player.saveAltar.NearToRespawn)
                {
                    buttonEquipUseTMP.text = "USAR";
                    buttonEquipUse.SetActive(true);
                    buttonRemove.SetActive(false);
                }
                else if (Inventary.Instance.InventaryItems[selectedSlot.Index].Type == ItemTypes.UpgradeItems)
                {
                    buttonEquipUseTMP.text = "USAR";
                    buttonEquipUse.SetActive(false);
                    buttonRemove.SetActive(false);
                }
                else if (Inventary.Instance.InventaryItems[selectedSlot.Index].IsSpecialItem)
                {
                    buttonEquipUse.SetActive(false);
                    buttonRemove.SetActive(false);
                }
                else if (Inventary.Instance.InventaryItems[selectedSlot.Index].Type == ItemTypes.Ninjutsus)
                {
                    buttonEquipUseTMP.text = "EQUIPAR";
                    buttonEquipUse.SetActive(true);
                    buttonRemove.SetActive(true);
                }
            }

            

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
