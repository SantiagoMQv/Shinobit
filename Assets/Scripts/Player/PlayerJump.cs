using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float staminaJump;

    private MovementPlayer movementPlayer;
    public static Action JumpPlayerEvent;
    private Rigidbody2D rigibody2D;
    private bool increaseSizePlayer;
    public bool CanJump { get; private set; }
    public bool Jumping { get; private set; }

    private StaminaPlayer staminaPlayer;
    private void Awake()
    {
        movementPlayer = GetComponent<MovementPlayer>();
        staminaPlayer = GetComponent<StaminaPlayer>();
        rigibody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        CanJump = true;
        increaseSizePlayer = false;
    }
    void Update()
    {
        UpdateSizePlayer();

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
        increaseSizePlayer = true;
        if (staminaPlayer.CurrentStamina <= 0)
        {
            movementPlayer.SetCanMove(false);
            yield return new WaitForSeconds(0.5f);
            increaseSizePlayer = false;
            yield return new WaitForSeconds(0.15f);
            Jumping = false;
            movementPlayer.SetCanMove(true);
        }
        else
        {
            movementPlayer.SetCanMove(false);
            staminaPlayer.SetCanBeRegenerate(false);
            yield return new WaitForSeconds(0.5f);
            increaseSizePlayer = false;
            yield return new WaitForSeconds(0.15f);
            Jumping = false;
            movementPlayer.SetCanMove(true);
            staminaPlayer.SetCanBeRegenerate(true);
        }
        
    }

    public void SetCanJump(bool result)
    {
        CanJump = result;
    }

    private void UpdateSizePlayer()
    {
        Vector2 bigScale = new Vector2(1.25f, 1.25f);
        Vector2 normalScale = new Vector2(1f, 1f);
        if (increaseSizePlayer)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, bigScale, 10 * Time.deltaTime);
        }
        else
        {
            transform.localScale = Vector2.Lerp(transform.localScale, normalScale, 10 * Time.deltaTime);
        }
    }
}
