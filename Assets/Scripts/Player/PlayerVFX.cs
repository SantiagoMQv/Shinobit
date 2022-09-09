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
        FloatingText floatingText = newTextGO.GetComponent<FloatingText>();
        floatingText.SetUpText(text, color);
        newTextGO.transform.SetParent(canvasTextPosition);
        newTextGO.transform.position = canvasTextPosition.position;
        newTextGO.SetActive(true);
        yield return new WaitForSeconds(1.17f);
        newTextGO.transform.SetParent(pooler.ListContainer.transform);
        newTextGO.SetActive(false);
        
    }

    private IEnumerator IECountdown(string text, Color color)
    {
        StartCoroutine(IEShowFloatingText("3", color, null));
        yield return new WaitForSeconds(1);
        StartCoroutine(IEShowFloatingText("2", color, null));
        yield return new WaitForSeconds(1);
        StartCoroutine(IEShowFloatingText("1", color, null));
        yield return new WaitForSeconds(1);
        StartCoroutine(IEShowFloatingText(text, color, null));
    }

    private void ResponseFloatingText(string text, Color color, GameObject enemy)
    {
        StartCoroutine(IEShowFloatingText(text, color, enemy));
    }

    private void CountdownResponse(string text, Color color)
    {
        StartCoroutine(IECountdown(text, color));
    }

    private void OnEnable()
    {
        HealthBase.EventFloatingText += ResponseFloatingText;
        Pickups.EventFloatingText += ResponseFloatingText;
        CombatPlayer.FloatingTextCountdownEvent += CountdownResponse;
    }

    private void OnDisable()
    {
        HealthBase.EventFloatingText -= ResponseFloatingText;
        Pickups.EventFloatingText -= ResponseFloatingText;
        CombatPlayer.FloatingTextCountdownEvent += CountdownResponse;
    }

}
