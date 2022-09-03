using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private int amoutToCreate;

    private List<GameObject> list;

    public GameObject ListContainer { get; private set; }

    public void CreatePooler(GameObject objectToCreate)
    {
        list = new List<GameObject>();
        ListContainer = new GameObject($"Pool - {objectToCreate.name}");

        for (int i = 0; i < amoutToCreate; i++)
        {
            list.Add(AddInstance(objectToCreate));
        }
    }

    private GameObject AddInstance(GameObject objectToCreate)
    {
        GameObject newObject = Instantiate(objectToCreate, ListContainer.transform);
        newObject.SetActive(false);
        return newObject;
    }

    public GameObject ObtainInstance()
    {
        for (int i = 0; i < list.Count; i++)
        {
            if(list[i].activeSelf == false)
            {
                return list[i];
            }
        }

        return null;
    }
}
