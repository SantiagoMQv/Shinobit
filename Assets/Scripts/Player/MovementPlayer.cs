using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    [SerializeField] private float speed;

    public bool moving => movementDirection.magnitude > 0;
    public Vector2 MovementDirection => movementDirection;

    private Rigidbody2D rigibody2D;
    private Vector2 input;
    private Vector2 movementDirection;

    // Start is called before the first frame update
    void Start()
    {
        rigibody2D = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

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

    //Usamos FixedUpdate porque trabajamos con un RigiBody2D
    private void FixedUpdate()
    {
        rigibody2D.MovePosition(rigibody2D.position + movementDirection * speed * Time.fixedDeltaTime);
    }
}
