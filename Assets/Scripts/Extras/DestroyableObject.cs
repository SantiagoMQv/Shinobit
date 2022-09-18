using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyableObject : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile") || collision.gameObject.CompareTag("ShurikenWeapon") || collision.gameObject.CompareTag("SpearWeapon"))
        {
            Destroy(this.gameObject);
        }
    }

}
