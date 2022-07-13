using UnityEngine;

public enum MovementDirections
{
    Horizontal,
    Vertical
}

public class WaypointMovement : MonoBehaviour
{
    
    [SerializeField] protected float speed;

    public Vector3 PointToMove => waypoint.GetMovementPosition(CurrentIndexPoint);

    private Waypoint waypoint;
    private int CurrentIndexPoint;
    protected Vector3 lastPosition;
    protected Animator animator;
    void Start()
    {
        CurrentIndexPoint = 0;
        waypoint = GetComponent<Waypoint>();
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        MoveCharacter();
        RotateHorizontal();
        RotateVertical();
    }

    private void MoveCharacter()
    {
        transform.position = Vector3.MoveTowards(transform.position, PointToMove, speed * Time.deltaTime);
        if (CheckCurrentPointReached())
        {
            UpdateMovementIndex();
        }
    }

    private bool CheckCurrentPointReached()
    {
        float distanceToCurrentPoint = (transform.position - PointToMove).magnitude;
        if(distanceToCurrentPoint < 0.1f)
        {
            lastPosition = transform.position;
            return true;
        }
        return false;
    }

    private void UpdateMovementIndex()
    {
        if(CurrentIndexPoint == waypoint.Points.Length - 1)
        {
            CurrentIndexPoint = 0;
        }
        else
        {
            if (CurrentIndexPoint <= waypoint.Points.Length - 1)
            {
                CurrentIndexPoint++;
            }
        }
    }

    protected virtual void RotateHorizontal()
    {
        
    }

    protected virtual void RotateVertical()
    {

    }

}
