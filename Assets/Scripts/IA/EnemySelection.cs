using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelection : MonoBehaviour
{
    [SerializeField] private GameObject vfxSelect;

    public void ShowSelectedEnemy(bool state)
    {
        vfxSelect.SetActive(state);
    }
}
