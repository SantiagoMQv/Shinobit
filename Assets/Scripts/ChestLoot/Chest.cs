using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, ISaveGame
{
    [SerializeField] private GameObject chestInteractButton;
    [SerializeField] private Sprite openedChest;
    [SerializeField] private string id;
    [SerializeField] private bool openedChestBool;

    // Permite desde el editor generar un ID identificativo que permitirá gestionar el guardado de datos para este objeto
    [ContextMenu("Generar guid para ID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }


    private bool playerNear;

    public string getID()
    {
        return id;
    }

    public bool getOpenedChestBool()
    {
        return openedChestBool;
    }

    private void Awake()
    {
       

        playerNear = false;
        if (openedChestBool)
        {
            OpenChest();
        }
       
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
        foreach (ChestData chest in data.chestData)
        {
            if (chest.ID == id)
            {
                openedChestBool = chest.openedChestBool;
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        foreach (ChestData chest in data.chestData)
        {
            if (chest.ID == id)
            {
                chest.openedChestBool = openedChestBool;
            }
        }
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
