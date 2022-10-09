using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LeverInteraction
{
    OpenChest
}
public class Lever : MonoBehaviour
{
    [SerializeField] private Sprite actionatedLeverSprite;
    [SerializeField] private LeverInteraction leverInteraction;
    [SerializeField] private Chest chest;
    private bool actionated;

    private void Update()
    {
        if (leverInteraction == LeverInteraction.OpenChest && actionated)
        {
            chest.OpenChest();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ShurikenWeapon") || collision.CompareTag("SpearWeapon"))
        {
            actionated = true;
            GetComponent<SpriteRenderer>().sprite = actionatedLeverSprite;
        }
    }
}
