using System;
using System.Collections.Generic;
using UnityEngine;

public enum QuizDifficulty
{
    Easy,
    Hard
}

public enum QuestionType
{
    Options,
    Fill
}

public enum QuizProgrammingLanguage
{
    Cplusplus
}

[CreateAssetMenu]
public class Quiz : ScriptableObject
{
    [Header("Info")]
    
    public string Name;
    public string ID;
    public int NumberQuestions;
    public QuizDifficulty quizDifficulty;
    public QuizProgrammingLanguage quizProgrammingLanguage;
    public NPCDialog NPCRelacionated;
    public int PossibleFails;

    [Header("Questions")]
    public TestQuestions[] quizQuestions;

    [Header("Description")]
    [TextArea] public string Description;

    [Header("Rewards")]
    public int BitsRewards;
    public QuizRewardItem quizRewardItem;
    [HideInInspector] public int QuestionAnswered;
    [HideInInspector] public int QuestionRight;
    [HideInInspector]public bool QuizCompleted;
    [HideInInspector]public bool QuizPickedUp;

}

[Serializable]
public class TestQuestions
{
    public QuestionType questionType;
    public string QuestionText;
    public Sprite GuideImage;
    [TextArea] public string CorrectAnswer;
    [TextArea] public string WrongAnswer1;
    [TextArea] public string WrongAnswer2;
}

[Serializable]
public class QuizRewardItem
{
    public GameObject Item;
    public int Amount;
}
