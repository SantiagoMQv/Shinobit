using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickups : Singleton<Pickups>, ISaveGame
{
    public static System.Action<string, Color, GameObject> EventFloatingText;
    public float CurrentBits { get; private set; }
    
    public int CurrentGoldKeys { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        CurrentBits = 0;
        CurrentGoldKeys = 0;
    }

    #region Keys

    public void AddGoldKeys(int amount)
    {
        CurrentGoldKeys += amount;
    }

    public void RemoveGoldKeys(int amount)
    {
        CurrentGoldKeys -= amount;
    }

    #endregion

    #region Bits

    public void AddBits(float amount)
    {
        CurrentBits += amount;
        EventFloatingText?.Invoke(amount.ToString() + " BITS", new Color32(2, 128, 0, 255), null);
    }


    public void RemoveBits(float amount)
    {
        CurrentBits -= amount;
    }

    public void LoadData(GameData data)
    {
        CurrentBits = data.currentBits;
        CurrentGoldKeys = data.currentGoldKeys;
    }

    public void SaveData(ref GameData data)
    {
        data.currentBits = CurrentBits;
        data.currentGoldKeys = CurrentGoldKeys;
    }

    #endregion
}
