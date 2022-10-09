using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData 
{
    public Vector3 respawnPosition;
    public int respawnSceneIndex;

    public float currentBits;
    public int currentGoldKeys;

    public ChestData[] chestData;
    public Dictionary<string, bool> pickedUpLoot;

    public PlayerStatsData playerStatsData;
    public ItemData[] itemData;

    public QuizData[] quizData;
    public int totalQuizsCompleted;

    public QuizProgrammingLanguage programmingLanguage;

    //public DestroyableElementData[] destroyableElementData;
    public Dictionary<string, bool> destroyableElements;
    public GameData()
    {
        respawnPosition = Vector3.zero;
        respawnSceneIndex = 1;

        pickedUpLoot = new Dictionary<string, bool>();
        playerStatsData = new PlayerStatsData();
        totalQuizsCompleted = 0;
        programmingLanguage = QuizProgrammingLanguage.Cplusplus;
        destroyableElements = new Dictionary<string, bool>();
    }

}
