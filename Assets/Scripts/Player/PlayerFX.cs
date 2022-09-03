using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    [SerializeField] private GameObject canvasFloatingTextPrefab;
    [SerializeField] private Transform canvasTextPosition;

    private ObjectPooler pooler;

    private void Awake()
    {
        pooler = GetComponent<ObjectPooler>();
    }

    private void Start()
    {
        pooler.CreatePooler(canvasFloatingTextPrefab);
    }

    private IEnumerator IEShowFloatingText(string text, Color color)
    {
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

    private void ResponseFloatingText(string text, Color color)
    {
        StartCoroutine(IEShowFloatingText(text, color));
    }

    private void OnEnable()
    {
        Healthbase.EventFloatingText += ResponseFloatingText;
        Pickups.EventFloatingText += ResponseFloatingText;
    }

    private void OnDisable()
    {
        Healthbase.EventFloatingText -= ResponseFloatingText;
        Pickups.EventFloatingText -= ResponseFloatingText;
    }

}
