using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class QuizDescription : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI quizNameTMP;
    [SerializeField] private TextMeshProUGUI quizDescriptionTMP;
    [SerializeField] private TextMeshProUGUI quizLanguajeTMP;
    [SerializeField] private TextMeshProUGUI quizDifficultyTMP;
    [SerializeField] private TextMeshProUGUI quizReward;

    public Quiz QuizLoad { get; set; }
    public void SetUpQuizUI(Quiz quiz)
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
            quizReward.text = $"-{quiz.BitsRewards} bits" + $"\n-{quiz.quizRewardItem.Item.Name} x {quiz.quizRewardItem.Amount}";
        }else
        {
            quizReward.text = $"-{quiz.BitsRewards} bits";
        }
        
    }

    public void UnSetQuizUI()
    {
        QuizLoad = null;
    }

    public void GetQuiz()
    {
        if(QuizLoad != null)
        {
            NinjaCodeManager.Instance.AddNinjaCode(QuizLoad);
            gameObject.SetActive(false);
        }
    }

}
