using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private GameObject npcInteractButton;
    [SerializeField] private NPCDialog npcDialog;

    [Header("Optional -> ExtraInteraction: Chest")]
    [SerializeField] private Chest chest;

    [Header("Optional -> ExtraInteraction: DestoyElement")]
    [SerializeField] private DestroyableElement elementToDestroy;

    [Header("Optional -> CheckRequirements: TotalQuizCompleted")]
    [SerializeField] private NPCDialog npcDialogMissingRequirements;
    [SerializeField] private int totalQuizCompletedRequirement;
    [SerializeField] private bool debugNotAplicateCheckRequeriments;

    [HideInInspector] public NPCDialog Dialog;

    public NPCDialog DialogMissingRequirements => npcDialogMissingRequirements;
    public Chest Chest => chest;
    public int TotalQuizCompletedRequirement => totalQuizCompletedRequirement;
    public DestroyableElement ElementToDestroy => elementToDestroy;

    public void LoadMainDialog()
    {
        Dialog = npcDialog;
    }
    public void LoadMissingRequirementsDialog()
    {
        Dialog = npcDialogMissingRequirements;
    }

    public void SetUpDialog()
    {
        if (DialogMissingRequirements != null && !debugNotAplicateCheckRequeriments)
        {
            if (TotalQuizCompletedRequirement <= NinjaCodeManager.Instance.TotalQuizCompleted)
            {
                LoadMainDialog();
            }
            else
            {
                LoadMissingRequirementsDialog();
            }
        }
        else
        {
            LoadMainDialog();
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            DialogueManager.Instance.NPCAvailable = this;
            npcInteractButton.SetActive(true);
            SetUpDialog();
            if (Dialog.IncludeExtraInteraction && Dialog.ExtraInteraction == ExtraInteractionNPC.Quiz)
            {
                NinjaCodeManager.Instance.LoadNinjaCodeSlotInGetPanel(Dialog);
            }
        }
    }
    

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DialogueManager.Instance.NPCAvailable = null;
            npcInteractButton.SetActive(false);
            SetUpDialog();
            if (Dialog.IncludeExtraInteraction && Dialog.ExtraInteraction == ExtraInteractionNPC.Quiz)
            {
                NinjaCodeManager.Instance.CleanNinjaCodeGetPanel();
            }
        }
    }

}
