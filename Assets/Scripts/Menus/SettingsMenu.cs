using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingsMenu : Singleton<SettingsMenu>, ISaveGame
{
    [Header("Audio")]
    [SerializeField] public AudioMixer audioMixer;
    [Header("Lenguaje de programación")]
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
        switch (languageIndex)
        {
            case 0:
                programmingLanguage = QuizProgrammingLanguage.Cplusplus;
                break;
            case 1:
                programmingLanguage = QuizProgrammingLanguage.Python;
                break;

        }
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
