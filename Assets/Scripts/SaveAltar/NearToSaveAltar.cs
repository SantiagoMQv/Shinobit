using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearToSaveAltar : MonoBehaviour
{
    private Player player;

    private void HealPlayerInSaveAltar()
    {
        player.HealthPlayer.RestoreHealth(10);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<Player>();
            InvokeRepeating("HealPlayerInSaveAltar", 0.5f, 0.5f);
            player.setNearToSpawn(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Inventary.Instance.healingNinjutsuItem != null)
            {
                Inventary.Instance.healingNinjutsuItem.AddAllHealthToken();
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.setNearToSpawn(false);
            player = null;
            CancelInvoke();
        }
    }
}
