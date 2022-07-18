using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaCodeInteractionNPC : MonoBehaviour
{
    private NPCInteraction npcInteraction;
    private void Start()
    {
        npcInteraction = GetComponent<NPCInteraction>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(npcInteraction.Dialog.IncludeExtraInteraction && npcInteraction.Dialog.ExtraInteraction == ExtraInteractionNPC.Test)
            {
                NinjaCodeManager.Instance.LoadNinjaCodeSlotInGetPanel(npcInteraction.Dialog);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (npcInteraction.Dialog.IncludeExtraInteraction && npcInteraction.Dialog.ExtraInteraction == ExtraInteractionNPC.Test)
            {
                NinjaCodeManager.Instance.CleanNinjaCodeGetPanel();
            }
        }
    }
}
