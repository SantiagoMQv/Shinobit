using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance 
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<T>();

                if(instance == null)
                {
                    GameObject newGO = new GameObject();
                    instance = newGO.AddComponent<T>();
                }
            }
            return instance;
        } 
    }


    protected virtual void Awake()
    {
        // Por alguna razón se crean dos instancias de Inventary que no he logrado identficar por qué, por eos tuve que crear
        // una condición especifica para esa situación, en un futuro se debería solucionar e identificar por qué se crean dos instancias
        // de Inventary 
        if (instance != null && instance != Inventary.instance)
        {
            Debug.LogWarning("Hay dos instancias de " + instance.ToString() + ", se borrará la instancia repetida...");
            Destroy(gameObject);
        }
        instance = this as T;
        
    }
}
