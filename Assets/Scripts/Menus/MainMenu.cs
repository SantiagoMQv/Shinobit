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
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ContinueGame()
    {
        SceneManager.LoadSceneAsync(SaveManager.Instance.RespawnSceneIndex);
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
