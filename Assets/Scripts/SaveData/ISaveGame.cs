using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveGame
{
    void LoadData(GameData data);
    void SaveData(ref GameData data);
}
