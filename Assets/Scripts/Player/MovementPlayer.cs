using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    [SerializeField] private float speedDefault;
    [SerializeField] private bool movementx2Debug;
    [HideInInspector] public float speed;

    public bool moving => movementDirection.magnitude > 0;
    public bool CanMove { get; private set; }
    public Vector2 MovementDirection => movementDirection;

    private Rigidbody2D _rigidbody2D;
    private Vector2 input;
    private Vector2 movementDirection;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        CanMove = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.B) && movementx2Debug)
        {
            speed = speedDefault * 2;
        }
        else
        {
            speed = speedDefault;
        }

        if (CanMove) { 
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            // X
            if(input.x > 0.1f)
            {
                movementDirection.x = 1f;
            }else if(input.x < 0)
            {
                movementDirection.x = -1f;
            }
            else
            {
                movementDirection.x = 0f;
            }

            // Y
            if (input.y > 0.1f)
            {
                movementDirection.y = 1f;
            }
            else if (input.y < 0)
            {
                movementDirection.y = -1f;
            }
            else
            {
                movementDirection.y = 0f;
            }
        }
        // Se ha hecho así para mantener el salto funcionando tal y como se pensó.
        else if(!CanMove && !Player.Instance.playerJump.Jumping)
        {
            movementDirection.x = 0f;
            movementDirection.y = 0f;
        }
    }

    //Usamos FixedUpdate porque trabajamos con un RigiBody2D
    private void FixedUpdate()
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + movementDirection * speed * Time.fixedDeltaTime);
    }

    public void SetCanMove(bool result)
    {
        CanMove = result;
    }
}
