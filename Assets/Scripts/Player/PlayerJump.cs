using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float staminaJump;

    private MovementPlayer movementPlayer;
    public static Action JumpPlayerEvent;

    public bool CanJump { get; private set; }
    public bool Jumping { get; private set; }

    private StaminaPlayer staminaPlayer;
    private void Awake()
    {
        movementPlayer = GetComponent<MovementPlayer>();
        staminaPlayer = GetComponent<StaminaPlayer>();
    }

    private void Start()
    {
        CanJump = true;
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && movementPlayer.CanMove && CanJump && movementPlayer.moving)
        {
            Jumping = true;
            staminaPlayer.UseStamina(staminaJump);
            DoJump();
            
        }
    }

    public void DoJump()
    {
        StartCoroutine("WaitForJump");
    }

    IEnumerator WaitForJump()
    {
        if(staminaPlayer.CurrentStamina <= 0)
        {
            movementPlayer.SetCanMove(false);
            yield return new WaitForSeconds(0.75f);
            Jumping = false;
            movementPlayer.SetCanMove(true);
        }
        else
        {
            movementPlayer.SetCanMove(false);
            staminaPlayer.SetCanBeRegenerate(false);
            yield return new WaitForSeconds(0.75f);
            Jumping = false;
            movementPlayer.SetCanMove(true);
            staminaPlayer.SetCanBeRegenerate(true);
        }
        
    }

    public void SetCanJump(bool result)
    {
        CanJump = result;
    }
}
