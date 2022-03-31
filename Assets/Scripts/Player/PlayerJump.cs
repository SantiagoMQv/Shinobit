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
    private SpriteRenderer spriteRenderer;

    private Vector2 bigScale;
    private Vector2 normalScale;
    private Color transparenceColor;
    private Color normalColor;
    private bool increaseSizePlayer;

    private float timeChangeTransparence;
    bool firstTimeJump;
    public bool CanJump { get; private set; }
    public bool Jumping { get; private set; }

    private StaminaPlayer staminaPlayer;
    private void Awake()
    {
        movementPlayer = GetComponent<MovementPlayer>();
        staminaPlayer = GetComponent<StaminaPlayer>();
        rigibody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        timeChangeTransparence = 0;
        CanJump = true;
        firstTimeJump = false;
        increaseSizePlayer = false;
        bigScale = new Vector2(1.25f, 1.25f);
        normalScale = new Vector2(1f, 1f);
        transparenceColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
        normalColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
    }
    void Update()
    {
        timeChangeTransparence += Time.deltaTime;
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
        timeChangeTransparence = 0;
        increaseSizePlayer = true;
        if (staminaPlayer.CurrentStamina <= 0)
        {
            movementPlayer.SetCanMove(false);    
            yield return new WaitForSeconds(0.515f);
            timeChangeTransparence = 0;
            increaseSizePlayer = false;
            yield return new WaitForSeconds(0.075f);
            Jumping = false;
            movementPlayer.SetCanMove(true);
        }
        else
        {
            movementPlayer.SetCanMove(false);
            staminaPlayer.SetCanBeRegenerate(false);
            yield return new WaitForSeconds(0.515f);
            timeChangeTransparence = 0;
            increaseSizePlayer = false;
            yield return new WaitForSeconds(0.075f);
            Jumping = false;
            movementPlayer.SetCanMove(true);
            staminaPlayer.SetCanBeRegenerate(true);
        }
        firstTimeJump = true;
    }

    public void SetCanJump(bool result)
    {
        CanJump = result;
    }

    private void UpdateSizePlayer()
    {
        
        if (increaseSizePlayer )
        {
            transform.localScale = Vector2.Lerp(transform.localScale, bigScale, 10 * Time.deltaTime);
            spriteRenderer.color = Color.Lerp(normalColor, transparenceColor, 5 * timeChangeTransparence);
        }
        else if(!increaseSizePlayer  && firstTimeJump)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, normalScale, 35 * Time.deltaTime);
            spriteRenderer.color = Color.Lerp(transparenceColor, normalColor, 2 * timeChangeTransparence);
        }
    }
}
