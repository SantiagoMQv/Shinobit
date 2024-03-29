using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject newGamePanel;

    [SerializeField] private Button continueButton;

    [SerializeField] private TMP_Dropdown programmingLanguageDropdown;

    private void Start()
    {
        SceneLoadManager.Instance.ActivateMainMenuTransition();
    }

    private void Update()
    {
        if (!SaveManager.Instance.ExistSaveGameFile())
        {
            continueButton.interactable = false;
            continueButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 100);
        }
    }

    public void OpenSettingsPanel()
    {
        if (programmingLanguageDropdown.options.Count  != System.Enum.GetValues(typeof(QuizProgrammingLanguage)).Length)
        {
            foreach (QuizProgrammingLanguage language in System.Enum.GetValues(typeof(QuizProgrammingLanguage)))
            {
                if (language.ToString() == QuizProgrammingLanguage.Cplusplus.ToString())
                {
                    programmingLanguageDropdown.options.Add(new TMP_Dropdown.OptionData("C++"));
                }
                else
                {
                    programmingLanguageDropdown.options.Add(new TMP_Dropdown.OptionData(language.ToString()));
                }
            }
        }
        
        SettingsMenu.Instance.SetProgrammingLanguage(programmingLanguageDropdown.value);
    }

    public void NewGameButton()
    {
        if (SaveManager.Instance.ExistSaveGameFile())
        {
            newGamePanel.SetActive(true);
        }
        else
        {
            NewGame();
        }
    }

    public void NewGame()
    {
        SaveManager.Instance.NewGame();
        SceneLoadManager.Instance.LoadNextScene();
    }

    public void ContinueGame()
    {
        SceneLoadManager.Instance.LoadDeterminatedScene(SaveManager.Instance.RespawnSceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenAssetsURL()
    {
        Application.OpenURL("https://pixel-boy.itch.io/ninja-adventure-asset-pack");
    }

    public void CloseNewGamePanel()
    {
        newGamePanel.SetActive(false);
    }
}
