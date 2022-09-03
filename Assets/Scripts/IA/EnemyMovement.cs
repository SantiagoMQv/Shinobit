using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : WaypointMovement
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(this.tag == "WaterEnemy" && collision.gameObject.tag == "Water")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
}
