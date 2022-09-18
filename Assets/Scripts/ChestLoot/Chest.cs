using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, ISaveGame
{
    [SerializeField] private GameObject chestInteractButton;
    [SerializeField] private Sprite openedChest;
    [SerializeField] private string id;

    // Permite desde el editor generar un ID identificativo que permitirá gestionar el guardado de datos para este objeto
    [ContextMenu("Generar guid para ID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private bool openedChestBool;
    private bool playerNear;

    private void Awake()
    {
        openedChestBool = false;
        playerNear = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && openedChestBool && playerNear)
        {
            Loot loot = GetComponent<Loot>();
            LootManager.Instance.ShowLootPanel(loot);
        }
    }
    public void OpenChest()
    {
        openedChestBool = true;
        GetComponent<SpriteRenderer>().sprite = openedChest;
    }

    public void LoadData(GameData data)
    {
        data.chestOpened.TryGetValue(id, out openedChestBool);
        if (openedChestBool)
        {
            OpenChest();
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.chestOpened.ContainsKey(id)){
            data.chestOpened.Remove(id);
        }
        data.chestOpened.Add(id, openedChestBool);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (openedChestBool)
        {
            if (collision.CompareTag("Player"))
            {
                chestInteractButton.SetActive(true);
                playerNear = true;
            }
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            chestInteractButton.SetActive(false);
            playerNear = false;
        }
    }

}
