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
    [SerializeField] private GameObject buttonsGO;
    [SerializeField] private GameObject fillerGO;

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
    public int TotalQuizPassed;

    private QuizSlotPlayerPanel currentSlot;
    private Quiz currentQuiz;

    private List<TextMeshProUGUI> mixButtons;
    private string playerAnswer;
    private GameObject currentButton;
    private bool lostGame;
    public bool quizCompleted;

    private void Start()
    {
        TotalQuizPassed = 0;
        quizCompleted = true;
        mixButtons = new List<TextMeshProUGUI>();
        mixButtons.Add(button1TMP);
        mixButtons.Add(button2TMP);
        mixButtons.Add(button3TMP);
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
        playerAnswer = null;
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
        playerAnswer = null;
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
            TotalQuizPassed++;
            DisplayInfoQuizPanel();
            return;
        }
        SetUpQuizUI();
    }

    #region FillField

    public void ReadStringInput(String s)

    {
        playerAnswer = s;
    }

    #endregion

    #region UI
    public void SetUpQuizUI()
    {
        fillerGO.SetActive(false);
        buttonsGO.SetActive(false);
        quizQuestionTMP.text = currentQuiz.quizQuestions[currentNumQuestion].QuestionText;
        quizImage.sprite = currentQuiz.quizQuestions[currentNumQuestion].GuideImage;
        if (currentQuiz.quizQuestions[currentNumQuestion].questionType == QuestionType.Options)
        {
            buttonsGO.SetActive(true);
            mixButtons.Shuffle();
            mixButtons[0].text = currentQuiz.quizQuestions[currentNumQuestion].CorrectAnswer;
            mixButtons[1].text = currentQuiz.quizQuestions[currentNumQuestion].WrongAnswer1;
            mixButtons[2].text = currentQuiz.quizQuestions[currentNumQuestion].WrongAnswer2;
        }else if (currentQuiz.quizQuestions[currentNumQuestion].questionType == QuestionType.Fill)
        {
            fillerGO.SetActive(true);
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
            titleTMP.text = "¡VICTORIA!";
            titleTMP.color = new Color32(11, 84, 16, 255);
            descriptionTMP.text = "¡Felicidades! Has obtenido:\n";
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
            titleTMP.text = "¡DERROTA!";
            titleTMP.color = new Color32(84, 21, 11, 255);
            descriptionTMP.text = "No has podido completar el reto :(\nSigue intentándolo para poder reclamar las recompensas y seguir progresando.";
            descriptionTMP.margin = new Vector4(0, 0, 0, 0);
            backgroundButton.color = new Color32(84, 21, 11, 255);
            textButton.color = new Color32(255, 153, 161, 255);
            textButton.text = "VOLVER";
        }

    }
    public void DisplayCorrectMessage()
    {
        messageAnswer.gameObject.SetActive(true);
        messageAnswer.text = "¡Respuesta correcta!";
        messageAnswer.color = new Color32(0, 161, 12, 255);
        currentButton.GetComponent<Image>().color = new Color32(32, 255, 43, 255);
    }
    public void DisplayWrongMessage()
    {
        messageAnswer.gameObject.SetActive(true);
        messageAnswer.text = "¡Incorrecto!";
        messageAnswer.color = new Color32(255, 0, 37, 255);
        currentButton.GetComponent<Image>().color = new Color32(255, 32, 58, 255);
    }
    public void InitializeAllButtons()
    {
        foreach (TextMeshProUGUI button in mixButtons)
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
        if(playerAnswer != null && !quizCompleted)
        {
            nextButton.SetActive(true);
            confirmButton.SetActive(false);
            ProcessAnswer();
        }
        
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

