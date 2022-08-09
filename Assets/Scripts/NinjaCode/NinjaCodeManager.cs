using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NinjaCodeManager : Singleton<NinjaCodeManager>
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
        QuizSlot newNinjaCode = Instantiate(NinjaCodeSlotPlayerPrefab, NinjaCodeSlotPlayerContainer);
        newNinjaCode.SetUpQuizSlotUI(quizToComplete);
        quizAvailableToGet.Remove(quizToComplete);
        quizAvailableToDo.Add(quizToComplete);
    }
    
    

}
