using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private GameObject DialogPanel;
    [SerializeField] private Image npcIcon;
    [SerializeField] private TextMeshProUGUI npcNameTMP;
    [SerializeField] private TextMeshProUGUI npcConversationTMP;

    public NPCInteraction NPCAvailable { get; set; }

    private Queue<string> dialogSequence;
    private bool animatedDialogEnded;
    private bool endDialogDisplayed;

    private void Start()
    {
        dialogSequence = new Queue<string>();
    }

    private void Update()
    {
        if(NPCAvailable != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SetUpPanel(NPCAvailable.Dialog);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (endDialogDisplayed)
            {
                OpenCloseDialogPanel(false);
                endDialogDisplayed = false;
                return;
            }
            if (animatedDialogEnded)
            {
                ContinueDialog();
            }
        }

    }

    public void OpenCloseDialogPanel(bool state)
    {
        DialogPanel.SetActive(state);
    }

    private void SetUpPanel(NPCDialog npcDialog)
    {
        OpenCloseDialogPanel(true);
        LoadDialogSequence(npcDialog);
        DisplayTextAnimated(npcDialog.BeginText);

        npcIcon.sprite = npcDialog.Icon;
        npcNameTMP.text = npcDialog.Name;
    }
    private void LoadDialogSequence(NPCDialog npcDialog)
    {
        if(npcDialog.Conversation == null || npcDialog.Conversation.Length <= 0)
        {
            return;
        }

        for (int i = 0; i < npcDialog.Conversation.Length; i++)
        {
            dialogSequence.Enqueue(npcDialog.Conversation[i].Sentence);
        }

    }

    private void ContinueDialog()
    {
        if(NPCAvailable != null || !endDialogDisplayed)
        {
            if (dialogSequence.Count == 0) // Si no queda ningún dialogo en la cola
            {
                string endDialog = NPCAvailable.Dialog.EndText;
                DisplayTextAnimated(endDialog);
                endDialogDisplayed = true;
                return;

            }
            string nextDialog = dialogSequence.Dequeue();
            DisplayTextAnimated(nextDialog);
        }
        
    }

    private IEnumerator AnimateText(string sentence)
    {
        animatedDialogEnded = false;
        npcConversationTMP.text = "";
        char[] letters = sentence.ToCharArray();
        for (int i = 0; i < letters.Length; i++)
        {
            npcConversationTMP.text += letters[i];
            yield return new WaitForSeconds(0.03f);
        }
        animatedDialogEnded = true;
    }

    private void DisplayTextAnimated(string sentence)
    {
        StartCoroutine(AnimateText(sentence));
    }
}
