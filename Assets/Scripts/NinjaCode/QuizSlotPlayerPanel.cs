using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizSlotPlayerPanel : QuizSlot
{
    public void StartQuiz()
    {
        if (QuizLoad != null)
        {
            QuizManager.Instance.StartQuiz(QuizLoad, this);
        }
    }

    public void FailedQuiz()
    {
        if (QuizLoad != null)
        {
            UIManager.Instance.OpenCloseNinjaCodeQuizPanel();
        }
    }

    public void CompletedQuiz()
    {
        if (QuizLoad != null)
        {
            UIManager.Instance.OpenCloseNinjaCodeQuizPanel();
            UIManager.Instance.OpenCloseNinjaCodePlayerPanel();
            gameObject.SetActive(false);
            if (QuizLoad.quizRewardItem.Item != null)
            {
                Instantiate(QuizLoad.quizRewardItem.Item, Player.Instance.transform.position + new Vector3(1,0,0) , QuizLoad.quizRewardItem.Item.transform.rotation);
            }
            Pickups.Instance.AddBits(QuizLoad.BitsRewards);
        }
    }
    
}
