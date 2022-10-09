using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    [SerializeField] private string layerDefault;
    [SerializeField] private string layerMainMenuTransition;
    [SerializeField] private string layerFadeTransition;

    private bool transitionLoading;
    private Animator transitionAnimator;


    private readonly int startingTransition = Animator.StringToHash("StartingTransition");
    private readonly int endingTransition = Animator.StringToHash("EndingTransition");

    public bool TransitionLoading => transitionLoading;
    private void Start()
    {
        transitionAnimator = GetComponentInChildren<Animator>();
    }

    public void ActivateMainMenuTransition()
    {
        ActivateLayer(layerMainMenuTransition);
    }


    public void LoadNextScene(){
        int nexSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(SceneLoad(nexSceneIndex));
    }

    public void LoadDeterminatedScene(int sceneIndex)
    {
        StartCoroutine(SceneLoad(sceneIndex));
    }

    public void ActivateFadeTransition()
    {
        StartCoroutine(IEActivateFadeTransition());
    }

    private IEnumerator IEActivateFadeTransition()
    {
        transitionLoading = true;
        int currentLayerIndex = transitionAnimator.GetLayerIndex(layerFadeTransition);

        ActivateLayer(layerFadeTransition);
        transitionAnimator.SetTrigger(startingTransition);

        yield return new WaitForSeconds(transitionAnimator.GetCurrentAnimatorStateInfo(currentLayerIndex).length);

        transitionAnimator.SetTrigger(endingTransition);

        yield return new WaitForSeconds(transitionAnimator.GetCurrentAnimatorStateInfo(currentLayerIndex).length);

        transitionLoading = false;
    }

    public IEnumerator SceneLoad(int sceneIndex)
    {
        // Lo he puesto así provisionalmente, si se añaden transiciones es necesario refactorizar esto
        transitionLoading = true;

        int currentLayerIndex = 0;
        if (sceneIndex != 0)
        {
            ActivateLayer(layerFadeTransition);
            currentLayerIndex = transitionAnimator.GetLayerIndex(layerFadeTransition);
        }
        else
        {
            
            currentLayerIndex = 0; // Menú principal
        }

        transitionAnimator.SetTrigger(startingTransition);
        yield return new WaitForSeconds(transitionAnimator.GetCurrentAnimatorStateInfo(currentLayerIndex).length);

        SceneManager.LoadSceneAsync(sceneIndex);

        if(sceneIndex != 0)
        {
            currentLayerIndex = transitionAnimator.GetLayerIndex(layerFadeTransition);
            transitionAnimator.SetTrigger(endingTransition);
        }
        else
        {
            ActivateLayer(layerMainMenuTransition);
            currentLayerIndex = 0;
        }

        yield return new WaitForSeconds(transitionAnimator.GetCurrentAnimatorStateInfo(currentLayerIndex).length);
        transitionLoading = false;
    }

    #region AnimationLayers
    private void ActivateLayer(string layerName)
    {
        for (int i = 0; i < transitionAnimator.layerCount; i++) //Desactiva las layers antes de activar una.
        {
            transitionAnimator.SetLayerWeight(i, 0); //Asigna un peso a un layer. 0 -> Desactivado.  1 -> Activa.
        }
        transitionAnimator.SetLayerWeight(transitionAnimator.GetLayerIndex(layerName), 1);
    }

    #endregion

}
