using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaCodeNPCInteraction : NPCInteraction
{
    private NPCInteraction npcInteraction;
    private void Start()
    {
        npcInteraction = GetComponent<NPCInteraction>();
    }



    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player"))
        {
            if(npcInteraction.Dialog.IncludeExtraInteraction && npcInteraction.Dialog.ExtraInteraction == ExtraInteractionNPC.Quiz)
            {
                NinjaCodeManager.Instance.LoadNinjaCodeSlotInGetPanel(npcInteraction.Dialog);
            }
        }
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.CompareTag("Player"))
        {
            if (npcInteraction.Dialog.IncludeExtraInteraction && npcInteraction.Dialog.ExtraInteraction == ExtraInteractionNPC.Quiz)
            {
                NinjaCodeManager.Instance.CleanNinjaCodeGetPanel();
            }
        }
    }
}
