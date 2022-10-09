using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transport : MonoBehaviour
{
    [SerializeField] private bool isFirstTransport;
    [SerializeField] private Camera MainCamera;

    private Vector3 position;
    

    public bool EnableToTransport { get; private set; }

    [SerializeField] private Transport ToTransport;

    private void Start()
    {
        position = transform.position;
        if (isFirstTransport)
        {
            EnableToTransport = true;
        }
    }

    public void SetEnableToTransport(bool state)
    {
        EnableToTransport = state;
    }

    private IEnumerator WaitToMove(Player player)
    {
        yield return new WaitForSeconds(1.5f);
        player.movementPlayer.SetCanMove(true);
    }

    private IEnumerator WaitToTransport(Player player)
    {
        yield return new WaitForSeconds(1);
        player.transform.position = ToTransport.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && EnableToTransport)
        {
            Player player = collision.GetComponent<Player>();
            ToTransport.SetEnableToTransport(false);

            StartCoroutine(WaitToTransport(player));
            
            player.movementPlayer.SetCanMove(false);

            StartCoroutine(WaitToMove(player));
            SceneLoadManager.Instance.ActivateFadeTransition();
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EnableToTransport = true;
        }
    }

}
