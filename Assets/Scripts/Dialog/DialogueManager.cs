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
    private bool endDialogEnded;
    private bool dialogFinished;
    private bool dialogStarted;
    private bool skipAnimation;

    private void Start()
    {
        dialogSequence = new Queue<string>();
        dialogFinished = true;
        skipAnimation = false;
        dialogStarted = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (endDialogEnded)
            {
                OpenCloseDialogPanel(false);
                InitializeVariables();
                CheckExtraInteraction();
                return;
            }
            if (dialogStarted)
            {

                if (animatedDialogEnded)
                {
                    ContinueDialog();
                }
                else
                {
                    skipAnimation = true;
                }
            }
            

        }

        if (NPCAvailable != null)
        {
            if (Input.GetKeyDown(KeyCode.E) && dialogFinished && !UIManager.Instance.DisplayingPanel)
            {
                dialogFinished = false;
                dialogStarted = true;
                Player.Instance.movementPlayer.SetCanMove(false);
                SetUpPanel(NPCAvailable.Dialog);
            }
        }

    }

    private void InitializeVariables()
    {
        skipAnimation = false;
        endDialogEnded = false;
        endDialogDisplayed = false;
        dialogFinished = true;
        dialogStarted = false;
    }

    private void CheckExtraInteraction()
    {
        if (NPCAvailable.Dialog.IncludeExtraInteraction)
        {
            if (NPCAvailable.Dialog.ExtraInteraction == ExtraInteractionNPC.Quiz) // Para las que necesitan UI
            {
                UIManager.Instance.OpenCloseInteraction(NPCAvailable.Dialog.ExtraInteraction);
            }
            else // No necesitan UI
            {
                if (NPCAvailable.Dialog.ExtraInteraction == ExtraInteractionNPC.OpenChest)
                {
                    NPCAvailable.Chest.OpenChest();
                }
                else if (NPCAvailable.Dialog.ExtraInteraction == ExtraInteractionNPC.DestroyElement)
                {
                    NPCAvailable.ElementToDestroy.DestoyElement();
                }
                Player.Instance.movementPlayer.SetCanMove(true);
            }

        }
        else
        {
            Player.Instance.movementPlayer.SetCanMove(true);
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
            if (dialogSequence.Count == 0) // Si no queda ning�n dialogo en la cola
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
            if (skipAnimation)
            {
                if(endDialogDisplayed || NPCAvailable.Dialog.EndText.ToString() == "")
                {
                    endDialogEnded = true;
                }
                npcConversationTMP.text = "";
                npcConversationTMP.text = sentence;
                animatedDialogEnded = true;
                skipAnimation = false;
                yield break;
            }
            npcConversationTMP.text += letters[i];
            yield return new WaitForSeconds(0.03f);
        }
        if (endDialogDisplayed)
        {
            endDialogEnded = true;
        }
        animatedDialogEnded = true;
    }

    private void DisplayTextAnimated(string sentence)
    {
        StartCoroutine(AnimateText(sentence));
    }
}
