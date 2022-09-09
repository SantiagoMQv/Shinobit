using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private Sprite actionatedLeverSprite;

    private bool actionated;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ShurikenWeapon"))
        {
            actionated = true;
            GetComponent<SpriteRenderer>().sprite = actionatedLeverSprite;
        }
    }
}
