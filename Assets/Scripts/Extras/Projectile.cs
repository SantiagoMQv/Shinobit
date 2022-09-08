using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileUserType
{
    Enemy,
    Player
}

public class Projectile : MonoBehaviour
{
    public float speed;
    public float damage;
    public ProjectileUserType userType;

    private Rigidbody2D _rigidbody2D;
    private Vector2 direction;
    private EnemySelection enemyTarget;

    private float timeLife;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        timeLife = Time.time + 300;

    }

    private void FixedUpdate()
    {
        if(userType == ProjectileUserType.Player)
        {
            MoveProjectile();
        }

        if (timeLife < Time.time)
        {
            if (userType == ProjectileUserType.Enemy)
            {
                Destroy(this.gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
           
        }
    }

    public void MoveProjectile()
    {
        if(enemyTarget != null)
        {
            _rigidbody2D.AddForce(direction.normalized * speed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
        else
        {

            _rigidbody2D.AddForce(direction.normalized * speed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
        
    }

    private Vector2 SelectShotDirection(int indexShotDirection)
    {
        Vector2 shotDirection = new Vector2(0,0);
        switch (indexShotDirection)
        {
            case 0:
                shotDirection = Vector3.up;
                break;
            case 1:
                shotDirection = Vector3.right;
                break;
            case 2:
                shotDirection = Vector3.down;
                break;
            case 3:
                shotDirection = Vector3.left;
                break;
        }
        return shotDirection;
    }

    public void InitializeProjectile(EnemySelection enemy, int indexShotDirection, float speedWeapon, float projectileTimeLife)
    {
        speed = speedWeapon;
        timeLife = Time.time + projectileTimeLife;
        if (enemy != null) // Si se está apuntando a un enemigo
        {
            enemyTarget = enemy;
            direction = (enemyTarget.transform.position - transform.position);
            transform.eulerAngles = new Vector3(0, 0, Utils.GetAngleFromVectorFloat(direction));
        }
        else 
        {
            Vector3 directionAng = SelectShotDirection(indexShotDirection);
            transform.eulerAngles = new Vector3(0, 0, Utils.GetAngleFromVectorFloat(directionAng));
            direction = transform.right;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && userType == ProjectileUserType.Enemy)
        {
            collision.gameObject.GetComponent<HealthPlayer>().GetDamage(damage, null);
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Enemy" && userType == ProjectileUserType.Player)
        {
            collision.gameObject.GetComponent<EnemyHealth>().GetDamage(Player.Instance.combatPlayer.GetDamage(), collision.gameObject);
            this.gameObject.SetActive(false);
        }

    }
}
