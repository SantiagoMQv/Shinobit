using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "WaterEnemy")
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<HealthPlayer>().GetDamage(damage);
            }
            Destroy(this.gameObject);
        }
        
    }
}
