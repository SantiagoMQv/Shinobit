using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : WaypointMovement
{
    [SerializeField] private MovementDirections direction;

    private readonly int walkDown = Animator.StringToHash("WalkDown");

    protected override void RotateHorizontal()
    {
        if (direction == MovementDirections.Horizontal)
        {
            if (PointToMove.x > lastPosition.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    protected override void RotateVertical()
    {
        if(direction == MovementDirections.Vertical)
        {
            if(PointToMove.y > lastPosition.y)
            {
                animator.SetBool(walkDown, false);
            }
            else
            {
                animator.SetBool(walkDown, true);
            }
        }
    }


}
