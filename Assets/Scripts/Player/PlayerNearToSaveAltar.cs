using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNearToSaveAltar : MonoBehaviour
{
    public bool NearToRespawn { get; private set; }
    private Player player;
    private void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {

    }

    private void HealPlayerInSaveAltar()
    {
        player.HealthPlayer.RestoreHealth(10);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            InvokeRepeating("HealPlayerInSaveAltar", 0.5f, 0.5f);
            NearToRespawn = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            player.AddAllHealthToken();
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            NearToRespawn = false;
            CancelInvoke();
        }
    }
}
