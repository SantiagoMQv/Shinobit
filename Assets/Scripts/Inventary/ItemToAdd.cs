using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemToAdd : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private InventaryItem inventaryItemReference;
    [SerializeField] private int amountToAdd;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Inventary.Instance.AddItem(inventaryItemReference, amountToAdd);
            Destroy(gameObject);
        }
    }
}
