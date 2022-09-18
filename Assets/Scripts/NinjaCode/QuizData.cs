using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuizData
{
    public string ID;
    public bool QuizPickedUp;
    public bool QuizCompleted;

    public QuizData()
    {
        QuizPickedUp = false;
        QuizCompleted = false;
    }

}
