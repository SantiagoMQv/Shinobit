using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private string layerIdle;
    [SerializeField] private string layerWalk;
    [SerializeField] private string layerJump;
    [SerializeField] private string layerSpell;
    [SerializeField] private string layerAttack;

    private Animator animator;
    private MovementPlayer movementPlayer;
    private PlayerJump playerJump;
    private Player player;
    //Hash de las animaciones
    private readonly int xDirection = Animator.StringToHash("X");
    private readonly int yDirection = Animator.StringToHash("Y");
    private readonly int defeated = Animator.StringToHash("Defeated");
    private readonly int healing = Animator.StringToHash("Healing");
    private void Awake()
    {
        animator = GetComponent<Animator>();
        movementPlayer = GetComponent<MovementPlayer>();
        playerJump = GetComponent<PlayerJump>();
        player = GetComponent<Player>();
    }


    void Update()
    {
        LayersUpdate();
        if (movementPlayer.moving)
        {
            animator.SetFloat(xDirection, movementPlayer.MovementDirection.x);
            animator.SetFloat(yDirection, movementPlayer.MovementDirection.y);
        }
        
    }

    private void ActivateLayer(string layerName)
    {
        for (int i = 0; i < animator.layerCount; i++) //Desactiva las layers antes de activar una.
        {
            animator.SetLayerWeight(i, 0); //Asigna un peso a un layer. 0 -> Desactivado.  1 -> Activa.
        }

        animator.SetLayerWeight(animator.GetLayerIndex(layerName), 1);
    }

    private void LayersUpdate()
    {
        if (!playerJump.Jumping)
        {
            if (player.combatPlayer.Healing)
            {
                ActivateLayer(layerSpell);
            }
            else
            {
                if (!player.combatPlayer.Attacking)
                {
                    if (movementPlayer.moving)
                    {
                        ActivateLayer(layerWalk);
                    }
                    else
                    {
                        ActivateLayer(layerIdle);
                    }
                }
                else
                {
                    ActivateLayer(layerAttack);
                }
                
            }
            
        }
        else
        {
            ActivateLayer(layerJump);
        }
       
    }

    public void PlayerRevive()
    {
        ActivateLayer(layerIdle);
        animator.SetBool(defeated, false);
    }


    private void DefeatedPlayerResponse()
    {
        //if (animator.GetLayerWeight(animator.GetLayerIndex(layerIdle)) == 1) //Esto es porque la animacion de defeated está en layerIdle
        //{
        //    animator.SetBool(defeated, true);
        //}
        animator.SetBool(defeated, true);
    }

    //Son necesarios los siguientes métodos para que una clase se pueda sobrescribir a un evento
    private void OnEnable()
    {
        HealthPlayer.DefeatedPlayerEvent += DefeatedPlayerResponse;

    }

    private void OnDisable()
    {
        HealthPlayer.DefeatedPlayerEvent -= DefeatedPlayerResponse;
    }

}
