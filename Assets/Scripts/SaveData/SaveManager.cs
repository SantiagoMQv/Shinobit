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

    private void RestartAllQuiz()
    {
        Quiz[] quizzes = Resources.LoadAll<Quiz>("");
        foreach (Quiz quiz in quizzes)
        {
            quiz.QuizPickedUp = false;
            quiz.QuizCompleted = false;
        }
    }
    
    private void RestartAllUpgradeItems()
    {
        AttackUpgradeItem[] items = Resources.LoadAll<AttackUpgradeItem>("");
        
        foreach (AttackUpgradeItem item in items)
        {
            item.bitsToUpgrade = 100;
        }
    }

    private void RestartScriptableObjects()
    {
        RestartAllQuiz();
        RestartAllUpgradeItems();
    }
    
    public void NewGame()
    {
        this.gameData = new GameData();
        RestartScriptableObjects();
        SaveGame();
        LoadGame();
        Debug.Log("Creando nueva partida...");
    }
    public void LoadGame()
    {
        
        if (dataHandler.Load() != null)
        {
            // Se carga data de un fichero usando el dataHandler
            this.gameData = dataHandler.Load();
        }

        // Crea un new game si el data es nulo y hemos configurado que se inicialice con propositos de debugging
        if (this.gameData == null && initializeDataIfNull)
        {
            //NewGame();
        }

        if(this.gameData == null)
        {
            Debug.LogError("Se necesita un archivo de guardado para cargar la partida...");
            return;
        }
        if(this.gameData.chestData == null || this.gameData.chestData.Length == 0)
        {
            InitializeChestData();
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

        var data = this.gameData;
        if (data != null && (data.chestData == null || data.chestData.Length == 0))
        {
            InitializeChestData();
        }
        foreach (ISaveGame saveGameObj in saveGameObjects)
        {
            
            saveGameObj.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
        Debug.Log("Guardando partida...");
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

    private void InitializeChestData()
    {
        gameData.chestData = new ChestData[FindObjectsOfType<Chest>().Length];
        int i= 0;
        foreach (Chest chest in FindObjectsOfType<Chest>())
        {
            gameData.chestData[i] = new ChestData(chest.getID());
            gameData.chestData[i].openedChestBool = chest.getOpenedChestBool();
            i++;
        }
    }

    public void SaveDataOnlyForProgrammingLanguageOption()
    {
        SettingsMenu.Instance.SaveData(ref gameData);
        SaveGame();
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
