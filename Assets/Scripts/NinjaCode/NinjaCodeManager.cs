using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NinjaCodeManager : Singleton<NinjaCodeManager>, ISaveGame
{
    [Header("NinjaCode")]
    [SerializeField] private List<Quiz> quizAvailableToGet;
    private List<Quiz> quizAvailableToDo;

    [Header("NinjaCodeSlotGetPanel")]
    [SerializeField] private QuizSlot NinjaCodeSlotPrefab;
    [SerializeField] private Transform NinjaCodeSlotContainer;

    [Header("NinjaCodeSlotPlayerPanel")]
    [SerializeField] private QuizSlot NinjaCodeSlotPlayerPrefab;
    [SerializeField] private Transform NinjaCodeSlotPlayerContainer;

    private ArrayList currentNinjaCodesLoaded;
    private List<Quiz> allQuiz;

    private int totalQuizsCompleted;
    public int TotalQuizCompleted => totalQuizsCompleted;

    protected override void Awake()
    {
        base.Awake();
        quizAvailableToDo = new List<Quiz>();
    }

    public void LoadNinjaCodeSlotInGetPanel(NPCDialog npcRelationated)
    {
        currentNinjaCodesLoaded = new ArrayList();
        for (int i = 0; i < quizAvailableToGet.Count; i++)
        {
            // Solo se mostrar�n los que sean del lenguaje que se haya configurado en settings
            if(SettingsMenu.Instance.programmingLanguage == quizAvailableToGet[i].quizProgrammingLanguage)
            {
                // Cargar� solo los test que est�n relacionados con el NPC que los carga.
                if(quizAvailableToGet[i].NPCRelacionated == npcRelationated)
                {
                    QuizSlot newNinjaCode = Instantiate(NinjaCodeSlotPrefab, NinjaCodeSlotContainer);
                    newNinjaCode.SetUpQuizSlotUI(quizAvailableToGet[i]);
                    currentNinjaCodesLoaded.Add(newNinjaCode);
                }
            }
            
        }
    }

    public void CleanNinjaCodeGetPanel()
    {
        foreach (QuizSlot ninjaCode in currentNinjaCodesLoaded)
        {
            Destroy(ninjaCode.gameObject);
            ninjaCode.UnSetQuizUI();
        }
        currentNinjaCodesLoaded.Clear();
    }

    public void AddToPlayerPanel(Quiz quizToComplete)
    {
        quizToComplete.QuizPickedUp = true;
        QuizSlot newNinjaCode = Instantiate(NinjaCodeSlotPlayerPrefab, NinjaCodeSlotPlayerContainer);
        newNinjaCode.SetUpQuizSlotUI(quizToComplete);
        quizAvailableToGet.Remove(quizToComplete);
        quizAvailableToDo.Add(quizToComplete);
    }

    public void AddTotalQuizsCompleted()
    {
        totalQuizsCompleted++;
    }


    public void LoadData(GameData data)
    {
        if (data.quizData == null)
        {
            initializeQuizData(ref data.quizData);
        }
        List<Quiz> quizToRemove = new List<Quiz>();
        foreach (Quiz quiz in new List<Quiz>(quizAvailableToGet))
        {
            for (int i = 0; i < data.quizData.Length; i++)
            {
                if (data.quizData[i].ID == quiz.ID)
                {
                    if (data.quizData[i].QuizPickedUp && !data.quizData[i].QuizCompleted)
                    {
                        AddToPlayerPanel(quiz);
                    }else if (data.quizData[i].QuizPickedUp && data.quizData[i].QuizCompleted)
                    {
                        quizAvailableToGet.Remove(quiz);
                    }
                }
            }
        }

        totalQuizsCompleted = data.totalQuizsCompleted;
    }

    public void SaveData(ref GameData data)
    {
        if (data.quizData == null)
        {
            initializeQuizData(ref data.quizData);
        }

        List<Quiz> allQuiz = getAllQuiz();
        int i = 0;
        foreach (Quiz quiz in allQuiz)
        {
            data.quizData[i].ID = quiz.ID;
            data.quizData[i].QuizCompleted = quiz.QuizCompleted;
            data.quizData[i].QuizPickedUp = quiz.QuizPickedUp;
            i++;
        }
        data.totalQuizsCompleted = totalQuizsCompleted;
    }

    private List<Quiz> getAllQuiz()
    {
        List<Quiz> newList = new List<Quiz>(quizAvailableToGet);
        if (quizAvailableToDo != null)
        {
            newList.AddRange(quizAvailableToDo);
        }
        return newList;
    }

    private void initializeQuizData(ref QuizData[] quizData)
    {
        quizData = new QuizData[getAllQuiz().Count];
        for (int i = 0; i < quizData.Length; i++)
        {
            quizData[i] = new QuizData();
        }
    }
}
