using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinToAdd : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int amountToAdd;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Inventary.Instance.AddBits(amountToAdd);
            Destroy(gameObject);
        }
    }
}
