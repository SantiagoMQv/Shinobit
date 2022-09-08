using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerVFX : MonoBehaviour
{
    [SerializeField] private GameObject canvasFloatingTextPrefab;
    [SerializeField] private Transform playerCanvasTextPosition;

    [Header("Pooler")]
    [SerializeField] private ObjectPooler pooler;

    private void Start()
    {
        pooler.CreatePooler(canvasFloatingTextPrefab);
    }


    private IEnumerator IEShowFloatingText(string text, Color color, GameObject enemy)
    {
        Transform canvasTextPosition = null;
        if (enemy == null)
        {
            canvasTextPosition = playerCanvasTextPosition;
        }else
        {
            GameObject enemyTextPosition = Utils.FindGameObjectInChildWithTag(enemy, "Position");
            canvasTextPosition = enemyTextPosition.transform;
            
        }
        GameObject newTextGO = pooler.ObtainInstance();
        Debug.Log(newTextGO);
        FloatingText floatingText = newTextGO.GetComponent<FloatingText>();
        floatingText.SetUpText(text, color);
        newTextGO.transform.SetParent(canvasTextPosition);
        newTextGO.transform.position = canvasTextPosition.position;
        newTextGO.SetActive(true);
        yield return new WaitForSeconds(1.17f);
        newTextGO.transform.SetParent(pooler.ListContainer.transform);
        newTextGO.SetActive(false);
        
    }

    private void ResponseFloatingText(string text, Color color, GameObject enemy)
    {
        StartCoroutine(IEShowFloatingText(text, color, enemy));
    }

    private void OnEnable()
    {
        HealthBase.EventFloatingText += ResponseFloatingText;
        Pickups.EventFloatingText += ResponseFloatingText;
    }

    private void OnDisable()
    {
        HealthBase.EventFloatingText -= ResponseFloatingText;
        Pickups.EventFloatingText -= ResponseFloatingText;
    }

}
