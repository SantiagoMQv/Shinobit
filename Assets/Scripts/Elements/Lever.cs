using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LeverInteraction
{
    OpenChest,
    DestroyElement
}
public class Lever : MonoBehaviour
{
    [SerializeField] private Sprite actionatedLeverSprite;
    [SerializeField] private LeverInteraction leverInteraction;
    [Header("Optional -> ExtraInteraction: OpenChest")]
    [SerializeField] private Chest chest;
    [Header("Optional -> ExtraInteraction: DestroyElement")]
    [SerializeField] private DestroyableElement element;
    
    private bool actionated;

    private void LeverAction()
    {
        if (leverInteraction == LeverInteraction.OpenChest && !actionated)
        {
            chest.OpenChest();
        }
        else if (leverInteraction == LeverInteraction.DestroyElement && !actionated)
        {
            element.DestoyElement();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ShurikenWeapon") || collision.CompareTag("SpearWeapon"))
        {
            LeverAction();
            actionated = true;
            GetComponent<SpriteRenderer>().sprite = actionatedLeverSprite;
        }
    }
}
