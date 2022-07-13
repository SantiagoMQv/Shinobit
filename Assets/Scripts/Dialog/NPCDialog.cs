using System;
using UnityEngine;

public enum ExtraInteractionNPC
{
    None,
    Test,
}

[CreateAssetMenu]
public class NPCDialog : ScriptableObject
{
    [Header("Info")]
    public string Name;
    public Sprite Icon;
    public bool IncludeExtraInteraction;
    public ExtraInteractionNPC ExtraInteraction;

    [Header("Begin")]
    [TextArea] public string BeginText;

    [Header("Chat")]
    public DialogueText[] Conversation;

    [Header("End")]
    [TextArea] public string EndText;

}

[Serializable]
public class DialogueText
{
    [TextArea] public string Sentence;
}
