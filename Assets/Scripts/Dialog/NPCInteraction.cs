using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private GameObject npcInteractButton;
    [SerializeField] private NPCDialog npcDialog;
    [SerializeField] private Chest chest;
    public NPCDialog Dialog => npcDialog;
    public Chest Chest => chest;

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DialogueManager.Instance.NPCAvailable = this;
            npcInteractButton.SetActive(true);

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

            if (Dialog.IncludeExtraInteraction && Dialog.ExtraInteraction == ExtraInteractionNPC.Quiz)
            {
                NinjaCodeManager.Instance.CleanNinjaCodeGetPanel();
            }
        }
    }

}
