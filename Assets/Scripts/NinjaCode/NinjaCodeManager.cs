using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaCodeManager : Singleton<NinjaCodeManager>
{
    [Header("NinjaCode")]
    [SerializeField] private List<Quiz> quizAvailableToGet;
    private List<Quiz> quizAvailableToDo;

    [Header("NinjaCodeSlotGetPanel")]
    [SerializeField] private QuizDescription NinjaCodeSlotPrefab;
    [SerializeField] private Transform NinjaCodeSlotContainer;

    [Header("NinjaCodeSlotPlayerPanel")]
    [SerializeField] private QuizDescription NinjaCodeSlotPlayerPrefab;
    [SerializeField] private Transform NinjaCodeSlotPlayerContainer;

    private ArrayList currentNinjaCodesLoaded;

    private void Start()
    {
        quizAvailableToDo = new List<Quiz>();
    }

    public void LoadNinjaCodeSlotInGetPanel(NPCDialog npcRelationated)
    {
        currentNinjaCodesLoaded = new ArrayList();
        for (int i = 0; i < quizAvailableToGet.Count; i++)
        {
            // Solo se mostrarán los que sean del lenguaje que se haya configurado en settings
            if(Settings.Instance.programmingLanguage == quizAvailableToGet[i].quizProgrammingLanguage)
            {
                // Cargará solo los test que estén relacionados con el NPC que los carga.
                if(quizAvailableToGet[i].NPCRelacionated == npcRelationated)
                {
                    QuizDescription newNinjaCode = Instantiate(NinjaCodeSlotPrefab, NinjaCodeSlotContainer);
                    newNinjaCode.SetUpQuizUI(quizAvailableToGet[i]);
                    currentNinjaCodesLoaded.Add(newNinjaCode);
                }
            }
            
        }
    }

    public void CleanNinjaCodeGetPanel()
    {
        foreach (QuizDescription ninjaCode in currentNinjaCodesLoaded)
        {
            Destroy(ninjaCode.gameObject);
            ninjaCode.UnSetQuizUI();
        }
        currentNinjaCodesLoaded.Clear();
    }

    private void AddNinjaCodeToComplete(Quiz quizToComplete)
    {
        QuizDescription newNinjaCode = Instantiate(NinjaCodeSlotPlayerPrefab, NinjaCodeSlotPlayerContainer);
        newNinjaCode.SetUpQuizUI(quizToComplete);
        quizAvailableToGet.Remove(quizToComplete);
        quizAvailableToDo.Add(quizToComplete);
    }

    public void AddNinjaCode(Quiz quizToComplete)
    {
        AddNinjaCodeToComplete(quizToComplete);
    }
    

}
