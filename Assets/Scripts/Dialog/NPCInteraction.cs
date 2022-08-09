using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private GameObject npcInteractButton;
    [SerializeField] private NPCDialog npcDialog;

    public NPCDialog Dialog => npcDialog;

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DialogueManager.Instance.NPCAvailable = this;
            npcInteractButton.SetActive(true);
        }
    }
    

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DialogueManager.Instance.NPCAvailable = null;
            npcInteractButton.SetActive(false);
        }
    }

}
