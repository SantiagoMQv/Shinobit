using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizSlotGetPanel : QuizSlot
{
    public void GetQuiz()
    {
        if (QuizLoad != null)
        {
            NinjaCodeManager.Instance.AddToPlayerPanel(QuizLoad);
            gameObject.SetActive(false);
        }
    }
}
