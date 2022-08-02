using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class QuizManager : Singleton<QuizManager>
{
    [Header("QuizPanel")]
    [SerializeField] private TextMeshProUGUI quizQuestionTMP;
    [SerializeField] private Image quizImage;
    [SerializeField] private TextMeshProUGUI button1TMP;
    [SerializeField] private TextMeshProUGUI button2TMP;
    [SerializeField] private TextMeshProUGUI button3TMP;
    [SerializeField] private TextMeshProUGUI possibleFailsTMP;
    [SerializeField] private TextMeshProUGUI currentQuestionTMP;
    [SerializeField] private TextMeshProUGUI totalQuestionTMP;
    [SerializeField] private TextMeshProUGUI messageAnswer;
    [SerializeField] private GameObject confirmButton;
    [SerializeField] private GameObject nextButton;

    [Header("InfoQuizPanel")]
    [SerializeField] private Image backgroundPanel;
    [SerializeField] private TextMeshProUGUI titleTMP;
    [SerializeField] private TextMeshProUGUI descriptionTMP;
    [SerializeField] private Image backgroundButton;
    [SerializeField] private TextMeshProUGUI textButton;

    private int possibleFails;
    private int totalQuestions;
    private int rightQuestions;
    private int currentNumQuestion;
    private bool quizCompleted;

    private QuizSlotPlayerPanel currentSlot;
    private Quiz currentQuiz;

    private List<TextMeshProUGUI> buttons;
    private string playerAnswer;
    private GameObject currentButton;
    private bool lostGame;

    private void Start()
    {
        buttons = new List<TextMeshProUGUI>();
        buttons.Add(button1TMP);
        buttons.Add(button2TMP);
        buttons.Add(button3TMP);
    }

    public void StartQuiz(Quiz quizToComplete, QuizSlotPlayerPanel slot)
    {
        UIManager.Instance.OpenCloseNinjaCodeQuizPanel();
        currentQuiz = quizToComplete;
        currentSlot = slot;
        InitializeValues();
        SetUpFailsQuizUI();
        SetUpQuizUI();
    }
    private void InitializeValues()
    {
        InitializeAllButtons();
        messageAnswer.gameObject.SetActive(false);
        nextButton.SetActive(false);
        confirmButton.SetActive(true);
        quizCompleted = false;
        possibleFails = currentQuiz.PossibleFails;
        totalQuestions = currentQuiz.quizQuestions.Count();
        rightQuestions = 0;
        currentNumQuestion = 0;
        lostGame = false;
    }
    public void ProcessAnswer()
    {
        if(playerAnswer == currentQuiz.quizQuestions[currentNumQuestion].CorrectAnswer)
        {
            rightQuestions++;
            DisplayCorrectMessage();
        }
        else
        {
            if (possibleFails > 0)
            {
                possibleFails--;
            }
            else
            {
                lostGame = true;
            }
            SetUpFailsQuizUI();
            DisplayWrongMessage();
        }
    }

    public void nextQuestion()
    {
        messageAnswer.gameObject.SetActive(false);
        InitializeAllButtons();
        currentNumQuestion++;
        if (lostGame)
        {
            DisplayInfoQuizPanel();
            return;
        }
        else if ((currentNumQuestion + 1) > totalQuestions)
        {
            quizCompleted = true;
            DisplayInfoQuizPanel();
            return;
        }
        SetUpQuizUI();
    }

    #region UI
    public void SetUpQuizUI()
    {
        quizQuestionTMP.text = currentQuiz.quizQuestions[currentNumQuestion].QuestionText;
        quizImage.sprite = currentQuiz.quizQuestions[currentNumQuestion].GuideImage;
        if (currentQuiz.quizQuestions[currentNumQuestion].questionType == QuestionType.Options)
        {
            buttons.Shuffle();
            buttons[0].text = currentQuiz.quizQuestions[currentNumQuestion].CorrectAnswer;
            buttons[1].text = currentQuiz.quizQuestions[currentNumQuestion].WrongAnswer1;
            buttons[2].text = currentQuiz.quizQuestions[currentNumQuestion].WrongAnswer2;
        }
        currentQuestionTMP.text = (currentNumQuestion + 1).ToString();
        totalQuestionTMP.text = $"/{totalQuestions.ToString()}";
    }

    public void SetUpFailsQuizUI()
    {
        possibleFailsTMP.text = possibleFails.ToString();
    }

    public void DisplayInfoQuizPanel()
    {
        UIManager.Instance.OpenCloseNinjaCodeInfoQuizPanel();
        if (quizCompleted)
        {
            backgroundPanel.color = new Color32(154, 255, 153, 255);
            titleTMP.text = "�VICTORIA!";
            titleTMP.color = new Color32(11, 84, 16, 255);
            descriptionTMP.text = "�Felicidades! Has obtenido:\n";
            if (currentQuiz.quizRewardItem.Item != null)
            {
                InventaryItem item = currentQuiz.quizRewardItem.Item.GetComponent<ItemToAdd>().getInventaryItemReference();
                descriptionTMP.text += $"- {currentQuiz.BitsRewards} bits" + $"\n-{item.Name} x {currentQuiz.quizRewardItem.Amount}";
            }
            else
            {
                descriptionTMP.text += $"- {currentQuiz.BitsRewards} bits";
            }
            descriptionTMP.margin = new Vector4(0, 15, 0, 0);
            backgroundButton.color = new Color32(11, 84, 16, 255);
            textButton.color = new Color32(154, 255, 153, 255);
            textButton.text = "RECLAMAR";
        }
        else
        {
            backgroundPanel.color = new Color32(255, 153, 161, 255);
            titleTMP.text = "�DERROTA!";
            titleTMP.color = new Color32(84, 21, 11, 255);
            descriptionTMP.text = "No has podido completar el reto :(\nSigue intent�ndolo para poder reclamar las recompensas y seguir progresando.";
            descriptionTMP.margin = new Vector4(0, 0, 0, 0);
            backgroundButton.color = new Color32(84, 21, 11, 255);
            textButton.color = new Color32(255, 153, 161, 255);
            textButton.text = "VOLVER";
        }

    }
    public void DisplayCorrectMessage()
    {
        messageAnswer.gameObject.SetActive(true);
        messageAnswer.text = "�Respuesta correcta!";
        messageAnswer.color = new Color32(0, 161, 12, 255);
        currentButton.GetComponent<Image>().color = new Color32(32, 255, 43, 255);
    }
    public void DisplayWrongMessage()
    {
        messageAnswer.gameObject.SetActive(true);
        messageAnswer.text = "�Incorrecto!";
        messageAnswer.color = new Color32(255, 0, 37, 255);
        currentButton.GetComponent<Image>().color = new Color32(255, 32, 58, 255);
    }
    public void InitializeAllButtons()
    {
        foreach (TextMeshProUGUI button in buttons)
        {
            GameObject buttonGO = button.transform.parent.gameObject;
            buttonGO.GetComponent<Image>().color = new Color32(43, 43, 43, 255);
        }
    }
    #endregion

    #region Buttons
    public void ButtonOnePressed()
    {
        playerAnswer = button1TMP.text;
        currentButton = button1TMP.transform.parent.gameObject;
    }
    public void ButtonTwoPressed()
    {
        playerAnswer = button2TMP.text;
        currentButton = button2TMP.transform.parent.gameObject;
    }
    public void ButtonThreePressed()
    {
        playerAnswer = button3TMP.text;
        currentButton = button3TMP.transform.parent.gameObject;
    }
    public void ConfirmButton()
    {
        nextButton.SetActive(true);
        confirmButton.SetActive(false);
        ProcessAnswer();
    }
    public void NextButton()
    {
        nextButton.SetActive(false);
        confirmButton.SetActive(true);
        nextQuestion();
    }
    public void ClaimReturnButton()
    {
        if (quizCompleted)
        {
            currentSlot.CompletedQuiz();
        }
        else
        {
            currentSlot.FailedQuiz();
        }
        UIManager.Instance.OpenCloseNinjaCodeInfoQuizPanel();
    }
    #endregion

}

static class ExtensionsClass
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

