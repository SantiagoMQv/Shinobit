using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AttackTypes
{
    Area,
    Melee,
    Dash,
    Projectile
}

public class IAController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private PlayerStats stats;

    [Header("States")]
    [SerializeField] private IAState initialState;
    [SerializeField] private IAState DefaultState;

    [Header("Config")]
    [SerializeField] private float detectionRange;
    [SerializeField] private float attackAreaRange;
    [SerializeField] private float meleeRange;
    [SerializeField] private float dashRange;
    [SerializeField] private float projectileRange;
    [SerializeField] private float speedMelee;
    [SerializeField] private float speedDash;
    [SerializeField] private float speedProjectile;
    [SerializeField] private float speedMovement;
    [SerializeField] private LayerMask playerLayerMask;

    [Header("Attack")]
    [SerializeField] private float damage;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private AttackTypes attackType;

    [Header("Prefabs")]
    [SerializeField] private GameObject projectilePrefab;

    [Header("Others")]
    [SerializeField] private bool WaterEnemy;

    [Header("Debug")]
    [SerializeField] private bool showDetection;
    [SerializeField] private bool showAttackAreaRange;
    [SerializeField] private bool showMeeleRange;
    [SerializeField] private bool showDashRange;
    [SerializeField] private bool showProjectileRange;

    private float timeToNextAttack;
    private bool attackDone;
    private bool attackDoing;
    private PolygonCollider2D polygonCollider2D;
    private Rigidbody2D _rigidbody2D;
    private IEnumerator DashCorroutine;

    public Transform PlayerReference { get; set; }
    public IAState CurrentState { get; set; }
    public EnemyMovement EnemyMovement { get; set; }
    public float DetectionRange => detectionRange;
    public float Damage => damage;
    public AttackTypes AttackType => attackType;
    public float SpeedMovement => speedMovement;
    public LayerMask PlayerLayerMask => playerLayerMask;
    public bool AttackDoing => attackDoing;

    private void Start()
    {
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        CurrentState = initialState;
        EnemyMovement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        CurrentState.ExecuteState(this);

        // Lo siguiente mantiene al enemigo interactuando con físicas dinámicas, pero lo vuelve kinematic cuando el jugador
        // se acerca para evitar problemas de colisiones
        SetKinematicWhenPlayerIsNear();
    }

    public void ChangeState(IAState newState)
    {
        if(newState != DefaultState)
        {
            CurrentState = newState;
        }
    }

    private void SetKinematicWhenPlayerIsNear()
    {
        if(PlayerReference != null)
        {
            float distanceTarget = Vector3.Distance(transform.position, PlayerReference.position);

            if (distanceTarget <= 1.5f)
            {
                _rigidbody2D.isKinematic = true;
                return;
            }
            else
            {
                _rigidbody2D.isKinematic = false;
            }
        }
        
    }

    public float AttackRangeDeterminated()
    {
        if(attackType == AttackTypes.Melee)
        {
            return meleeRange;
        }else if (attackType == AttackTypes.Dash)
        {
            return dashRange;
        }
        else if (attackType == AttackTypes.Projectile)
        {
            return projectileRange;
        }
        else
        {
            return attackAreaRange;
        }
    }

    public void AreaAttack()
    {
        if(PlayerReference != null)
        {
            ApplyDamageToPlayer(damage);
        }
    }

    public void MeleeAttack()
    {
        StartCoroutine(IEMelee());
    }

    public void DashAttack()
    {
        DashCorroutine = IEDash();
        StartCoroutine(DashCorroutine);
    }

    public void ProjectileAttack()
    {
        Vector3 playerPosition = PlayerReference.position;
        Vector3 initialPosition = transform.position;
        Vector3 directionToPlayer = (playerPosition - initialPosition).normalized;
        GameObject projectile = Instantiate(projectilePrefab, transform.position + directionToPlayer * 0.8f, Quaternion.identity);
        projectile.GetComponent<Projectile>().userType = ProjectileUserType.Enemy;
        projectile.GetComponent<Projectile>().damage = damage;
        projectile.GetComponent<Rigidbody2D>().AddForce(directionToPlayer * speedProjectile, ForceMode2D.Impulse);
        projectile.transform.eulerAngles = new Vector3(0,0, Utils.GetAngleFromVectorFloat(directionToPlayer));
        Destroy(projectile, 3f);
    }

    private IEnumerator IEMelee()
    {
        Vector3 playerPosition = PlayerReference.position;
        Vector3 initialPosition = transform.position;
        Vector3 directionToPlayer = (playerPosition - initialPosition).normalized;
        Vector3 attackPosition = playerPosition;

        
        //polygonCollider2D.enabled = false;

        float attackTransition = 0;
        while (attackTransition <= 1)
        {
            // Formula que permite que el enemigo vaya de la posición inicial hacia la posición del personaje y luego regrese a su posición inicial
            attackTransition += Time.deltaTime * speedMelee;
            float interpolation = (-Mathf.Pow(attackTransition, 2) + attackTransition) * 4;

            transform.position = Vector3.Lerp(initialPosition, attackPosition, interpolation );
            yield return null; // Permite esperar 1 frame
        }

        attackDone = false;
        //polygonCollider2D.enabled = true;
    }

    private IEnumerator IEDash()
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 playerPosition = PlayerReference.position;
        Vector3 initialPosition = transform.position;
        Vector3 directionToPlayer = (playerPosition - initialPosition).normalized;
        Vector3 attackPosition = playerPosition + directionToPlayer * 1f;

        attackDoing = true;
        //polygonCollider2D.enabled = false;

        
        float attackTransition = 0;
        while (attackTransition <= 1)
        {
            attackTransition += Time.deltaTime * speedDash;

            transform.position = Vector3.Lerp(initialPosition, attackPosition, attackTransition);
            yield return null; // Permite esperar 1 frame
        }
        attackDoing = false;
        attackDone = false;
        //polygonCollider2D.enabled = true;
    }

    private IEnumerator IECrashedWithObstacle()
    {
        float speedMovementAux = speedMovement;
        speedMovement = 0;

        // Necesario para que no se quede en la misma posición desde se paró ya que ese ocasionaba errores que le permitía atravesar obstáculos
        Vector3 playerPosition = PlayerReference.position;
        Vector3 initialPosition = transform.position;
        Vector3 directionToPlayer = (playerPosition - initialPosition).normalized;
        Vector3 newPosition = transform.position - directionToPlayer * 0.5f;
        float Transition = 0;
        while (Transition <= 1)
        {
            Transition += Time.deltaTime * speedDash;

            transform.position = Vector3.Lerp(initialPosition, newPosition, Transition);
            yield return null;
        }
        speedMovement = speedMovementAux;
    }

    public void ApplyDamageToPlayer(float amount)
    {
        if (!Player.Instance.playerJump.Jumping)
        {
            float damageToDo = 0;
            if (stats.Defense > 1)
            {
                damageToDo = damage - Mathf.Round(damage * (stats.Defense - 1) / 10);
            }
            else
            {
                damageToDo = damage;
            }
            PlayerReference.GetComponent<HealthPlayer>().GetDamage(damageToDo, null);
        }
    }

    public bool PlayerInRangeAttack(float range)
    {
        // Usamos sqrMagnitude ya que devuelve magnitude^2, que es más optimo para comparar posiciones.
        float distanceToPlayer = (PlayerReference.position - transform.position).sqrMagnitude;
        if (distanceToPlayer < Mathf.Pow(range, 2))
        {
            return true;
        }

        return false;
    }

    public bool IsAttackTime()
    {
        if(Time.time > timeToNextAttack)
        {
            return true;
        }
        return false;
    }

    public void UpdateTimeBetweenAttacks()
    {
        timeToNextAttack = Time.time + timeBetweenAttacks;
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" && !attackDone)
        {
            attackDone = true;
            ApplyDamageToPlayer(damage);
        }
        
        if(collision.transform.tag == "Obstaculo" && attackDoing && attackType == AttackTypes.Dash)
        {
            StopCoroutine(DashCorroutine);
            StartCoroutine(IECrashedWithObstacle());
            attackDoing = false;
            attackDone = false;
        }

        if (this.WaterEnemy && collision.gameObject.tag == "Water")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

    }

    private void OnDrawGizmos()
    {
        if (showDetection)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
        if (showAttackAreaRange)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, attackAreaRange);
        }
        if (showMeeleRange)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, meleeRange);
        }
        if (showDashRange)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, dashRange);
        }
        if (showProjectileRange)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, projectileRange);
        }
    }


}
