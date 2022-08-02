using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class QuizSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI quizNameTMP;
    [SerializeField] private TextMeshProUGUI quizDescriptionTMP;
    [SerializeField] private TextMeshProUGUI quizLanguajeTMP;
    [SerializeField] private TextMeshProUGUI quizDifficultyTMP;
    [SerializeField] private TextMeshProUGUI quizReward;

    public Quiz QuizLoad { get; set; }

    public void SetUpQuizSlotUI(Quiz quiz)
    {
        QuizLoad = quiz;
        quizNameTMP.text = quiz.Name;
        quizDescriptionTMP.text = quiz.Description;
        if(quiz.quizProgrammingLanguage == QuizProgrammingLanguage.Cplusplus)
        {
            quizLanguajeTMP.text = "C++";
        }
        else
        {
            quizLanguajeTMP.text = quiz.quizProgrammingLanguage.ToString();
        }
        if (quiz.quizDifficulty == QuizDifficulty.Hard)
        {
            quizDifficultyTMP.text = quiz.quizDifficulty.ToString();
            quizDifficultyTMP.color = new Color32(239, 70, 60, 255);
        }
        else
        {
            quizDifficultyTMP.text = quiz.quizDifficulty.ToString();
        }
        if (quiz.quizRewardItem.Item != null)
        {
            InventaryItem item = quiz.quizRewardItem.Item.GetComponent<ItemToAdd>().getInventaryItemReference();
            quizReward.text = $"-{quiz.BitsRewards} bits" + $"\n-{item.Name} x {quiz.quizRewardItem.Amount}";
        }else
        {
            quizReward.text = $"-{quiz.BitsRewards} bits";
        }
    }

    public void UnSetQuizUI()
    {
        QuizLoad = null;
    }

    

}
