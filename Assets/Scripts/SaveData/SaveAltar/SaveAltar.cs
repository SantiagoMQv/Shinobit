using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveAltar : MonoBehaviour
{
    [SerializeField] private GameObject saveGameNotice;

    private Player player;
    private int currentSceneIndex;
    private Vector3 respawnPosition;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        respawnPosition = transform.position + new Vector3(0, -1, 0);
    }

    private void HealPlayerInSaveAltar()
    {
        player.HealthPlayer.RestoreHealth(10);
    }

    private void AltarSaveGame()
    {
        saveGameNotice.SetActive(true);
        SaveManager.Instance.SaveGame();
    }

    private void SetRespawnPlayerValues()
    {
        player.respawnPosition = this.respawnPosition;
        player.respawnSceneIndex = this.currentSceneIndex;
    }

    private void PlayerDetected(Player player)
    {
        this.player = player;
        InvokeRepeating("HealPlayerInSaveAltar", 0.5f, 0.5f);
        player.setNearToSpawn(true);
    }

    private void PlayerUndetected()
    {
        saveGameNotice.SetActive(false);
        player.setNearToSpawn(false);
        player = null;
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerDetected(collision.gameObject.GetComponent<Player>());
            SetRespawnPlayerValues();
            AltarSaveGame();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.Instance.combatPlayer.AddAllHealthToken();
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerUndetected();
        }
    }

}
