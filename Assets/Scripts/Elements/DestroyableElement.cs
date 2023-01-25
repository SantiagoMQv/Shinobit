using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableElement : MonoBehaviour, ISaveGame
{
    [SerializeField] private string id;

    private bool elementDestroyed;

    // Permite desde el editor generar un ID identificativo que permitirá gestionar el guardado de datos para este objeto
    [ContextMenu("Generar guid para ID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    public void DestoyElement()
    {
        this.gameObject.SetActive(false);
        elementDestroyed = true;
    }

    public void LoadData(GameData data)
    {
        data.destroyableElements.TryGetValue(id, out elementDestroyed);
        if (elementDestroyed)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.destroyableElements.ContainsKey(id))
        {
            data.destroyableElements.Remove(id);
        }
        data.destroyableElements.Add(id, elementDestroyed);
    }
}
