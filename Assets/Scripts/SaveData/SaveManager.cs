using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;


    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData gameData;
    private List<ISaveGame> saveGameObjects;
    private FileDataHandler dataHandler;

    public int RespawnSceneIndex => gameData.respawnSceneIndex;

    protected override void Awake()
    {
        
        base.Awake();
        
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        SaveGame();
        LoadGame();
        Debug.Log("Creando nueva partida...");
    }
    public void LoadGame()
    {
        if(dataHandler.Load() != null)
        {
            // Se carga data de un fichero usando el dataHandler
            this.gameData = dataHandler.Load();
        }
        

        // Crea un new game si el data es nulo y hemos configurado que se inicialice con propositos de debugging
        if(this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        if(this.gameData == null)
        {
            Debug.LogError("Se necesita un archivo de guardado para cargar la partida...");
            return;
        }

        foreach (ISaveGame saveGameObj in saveGameObjects)
        {
            saveGameObj.LoadData(gameData);
        }
    }
    public void SaveGame()
    {
        if(this.gameData == null)
        {
            Debug.LogWarning("No se han encontrado datos del juego, se necesita empezar una partida nueva.");
        }
        foreach (ISaveGame saveGameObj in saveGameObjects)
        {
            
            saveGameObj.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
        Debug.Log("Guardando partida...");
        //Debug.Log(gameData.playerPosition);
    }

    public void UpdateSaveGameObjects()
    {
        this.saveGameObjects = FindAllSaveGameObjects();
    }

    private List<ISaveGame> FindAllSaveGameObjects()
    {
        
        IEnumerable<ISaveGame> saveGameObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveGame>();
        return new List<ISaveGame>(saveGameObjects);
    }

    public bool ExistSaveGameFile()
    {
        return dataHandler.ExistSaveGameFile();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        this.saveGameObjects = FindAllSaveGameObjects();
        LoadGame();

    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }

}
