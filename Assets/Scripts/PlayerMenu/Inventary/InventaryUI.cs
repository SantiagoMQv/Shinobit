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

    [Header("Botones")]
    [SerializeField] private TextMeshProUGUI buttonEquipUseTMP;
    [SerializeField] private GameObject buttonEquipUse;
    [SerializeField] private GameObject buttonRemove;
    [SerializeField] private GameObject BitsToUpgradeGO;
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup; //Necesario para ajustar el espaciado entre botones y texto en determinados casos
    [SerializeField] private TextMeshProUGUI BitsNecessaryTMP;

    [Header("Principal")]
    [SerializeField] private InventarySlot slotPrefab;
    [SerializeField] private Transform container;

    [Header("Weapons")]
    [SerializeField] private GameObject whereEquipPanel;

    public InventarySlot selectedSlot { get; private set; }
    private List<InventarySlot> availableSlot;

    protected override void Awake()
    {
        base.Awake();
        availableSlot = new List<InventarySlot>();
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
        if (gameObejectSelected == null)
        {
            inventaryDescriptionPanel.SetActive(false);
            return;
        }

        InventarySlot slot = gameObejectSelected.GetComponent<InventarySlot>();
        if(slot != null)
        {
            // Si el slot es de tipo UpgradeItem y no tiene ninguno, no se selecciona
            if (Inventary.Instance.InventaryItems[slot.Index].Type == ItemType.UpgradeItem && Inventary.Instance.InventaryItems[slot.Index].Amount < 1)
            {
                inventaryDescriptionPanel.SetActive(false);
            }
            else
            {
                selectedSlot = slot;
                UpdateInventaryDescription(selectedSlot.Index);
                // Dibujar botones
                UpdateButtons(selectedSlot.Index);
            }
            
        }

    }


    public void DrawnItemInInventary(InventaryItem itemToAdd, int amount, int itemIndex)
    {
        InventarySlot slot = availableSlot[itemIndex];
        if (itemToAdd != null)
        {
            if (itemToAdd.Type == ItemType.UpgradeItem && amount < 1)
            {
                slot.UpdateSlot(itemToAdd, amount);
                slot.ActivateItemIcon(true); // Asegurándonos de que el ícono esté activado
                slot.SetItemIconColor(Color.gray); // Hacer el ícono gris
                slot.ActivateBackgroundAmount(false);
            }
            else
            {
                slot.ActivateItemIcon(true);
                if (amount > 1)
                {
                    slot.ActivateBackgroundAmount(true);
                }
                slot.UpdateSlot(itemToAdd, amount);
                slot.SetItemIconColor(Color.white); // Asegurarse de que el ícono no esté gris cuando hay ítems
            }
        }
        else
        {
            slot.ActivateItemIcon(false);
            slot.ActivateBackgroundAmount(false);
        }
    }

    public void UpdateInventaryDescription(int index)
    {
        if (Inventary.Instance.InventaryItems[index] != null)
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
    #region Buttons

    public void UpdateButtons(int index)
    {
        if (Inventary.Instance.InventaryItems[index] != null)
        {
            if (Inventary.Instance.InventaryItems[index].Type == ItemType.UpgradeItem && Inventary.Instance.Player.NearToRespawn)
            {
                UpgradeItem upgradeItem = (UpgradeItem)Inventary.Instance.InventaryItems[index];
                string buttonText = "";
                if (Pickups.Instance.CurrentBits < upgradeItem.bitsToUpgrade)
                {
                    buttonText = "BLOQUEADO";
                    buttonEquipUseTMP.color = Color.red;
                    buttonEquipUse.GetComponent<Image>().color = Color.red;
                    buttonEquipUseTMP.enableAutoSizing = true;
                }
                else
                {
                    buttonText = "USAR";
                    buttonEquipUseTMP.color = Color.green;
                    buttonEquipUse.GetComponent<Image>().color = Color.green;
                    buttonEquipUseTMP.enableAutoSizing = false;
                }
                ModifyButtons(buttonText, true, true, false);
                BitsNecessaryTMP.text = upgradeItem.bitsToUpgrade.ToString();
                verticalLayoutGroup.spacing = -86;
            }
            else
            {
                buttonEquipUseTMP.enableAutoSizing = false;
                buttonEquipUseTMP.color = Color.green;
                buttonEquipUse.GetComponent<Image>().color = Color.green;

                verticalLayoutGroup.spacing = -16.6f;
                if (Inventary.Instance.InventaryItems[index].Type == ItemType.UpgradeItem)
                {
                    ModifyButtons("USAR", false, false, false);
                }
                else if (Inventary.Instance.InventaryItems[index].IsSpecialItem)
                {
                    ModifyButtons("", false, false, false);
                }
                else if (Inventary.Instance.InventaryItems[index].Type == ItemType.Weapon)
                {
                    ModifyButtons("EQUIPAR", true, false, true);
                }
            }
        }
    }

    private void ModifyButtons(string buttonText, bool equipUse, bool bitsToUpgrate, bool remove)
    {
        buttonEquipUseTMP.text = buttonText;
        buttonEquipUse.SetActive(equipUse);
        BitsToUpgradeGO.SetActive(bitsToUpgrate);
        buttonRemove.SetActive(remove);
    }

    public void UseEquipItem()
    {
        if (selectedSlot != null)
        {
            selectedSlot.UseEquipItemSlot();
            selectedSlot.SelectSlot();
        }
    }

    public void OpenCloseWhereEquipPanel()
    {
        whereEquipPanel.SetActive(true);
    }

    public void EquipWeaponContainer1()
    {
        if (selectedSlot != null)
        {
            selectedSlot.EquipWeaponContainer1();
            selectedSlot.SelectSlot();
        }
        whereEquipPanel.SetActive(false);
    }

    public void EquipWeaponContainer2()
    {
        if (selectedSlot != null)
        {
            selectedSlot.EquipWeaponContainer2();
            selectedSlot.SelectSlot();
        }
        whereEquipPanel.SetActive(false);
    }

    public void RemoveItem()
    {
        if (selectedSlot != null)
        {
            selectedSlot.RemoveItemSlot();
            selectedSlot.SelectSlot();
        }
    }

    #endregion

    #region Events
    private void SlotInteractionResponse(InteractionType type, int index)
    {
        if (type == InteractionType.Click)
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
    #endregion

   
}
