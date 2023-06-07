using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingsMenu : Singleton<SettingsMenu>, ISaveGame
{
    [Header("Audio")]
    [SerializeField] public AudioMixer audioMixer;
    [Header("Lenguaje de programaci�n")]
    [SerializeField] public QuizProgrammingLanguage programmingLanguage;

    private List<Button> menuButtons;

    protected override void Awake()
    {
        base.Awake();
        programmingLanguage = QuizProgrammingLanguage.Cplusplus;
    }

    private void Start()
    {
        initializeMenuButtons();
    }

    private void initializeMenuButtons()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            menuButtons.Add(transform.GetChild(i).GetComponent<Button>()) ;
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetProgrammingLanguage(int languageIndex)
    {
        int i = 0;
        foreach (QuizProgrammingLanguage language in System.Enum.GetValues(typeof(QuizProgrammingLanguage)))
        {
            if (i == languageIndex)
            {
                programmingLanguage = language;
            }
            i++;
        }
        SaveManager.Instance.SaveDataOnlyForProgrammingLanguageOption();
    }

    public void LoadData(GameData data)
    {
        programmingLanguage = data.programmingLanguage;
    }

    public void SaveData(ref GameData data)
    {
        data.programmingLanguage = programmingLanguage;
    }
}
