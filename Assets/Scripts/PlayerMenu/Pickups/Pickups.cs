using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickups : Singleton<Pickups>
{
    
    public float CurrentBits { get; set; }
    
    public int CurrentGoldKeys { get; set; }

    public GameObject FloatingTextPrefab;

    
    private void Start()
    {
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
        if (FloatingTextPrefab)
        {
            UIManager.Instance.ShowFloatingText(amount.ToString() + " BITS");
        }
    }


    public void RemoveBits(float amount)
    {
        CurrentBits -= amount;
    }

    #endregion
}
