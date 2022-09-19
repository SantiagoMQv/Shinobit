using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DontDestroy : MonoBehaviour
{
    private void Awake()
    {

        DontDestroyOnLoad(this.gameObject);
    }

    private void DestoyResponse()
    {
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
    }

    private void OnEnable()
    {
        PauseMenu.ReturnToMainMenuAction += DestoyResponse;
        DeathManager.ReviveLoadSceneAction += DestoyResponse;
    }

    private void OnDisable()
    {
        PauseMenu.ReturnToMainMenuAction -= DestoyResponse;
        DeathManager.ReviveLoadSceneAction -= DestoyResponse;
    }

}
