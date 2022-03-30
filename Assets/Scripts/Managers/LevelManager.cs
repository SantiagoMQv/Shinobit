using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform reappearancePoint;
    [SerializeField] private Player player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (player.HealthPlayer.Defeated)
            {
                player.transform.localPosition = reappearancePoint.position;
                player.PlayerRestore();
            }
        }
    }
}
