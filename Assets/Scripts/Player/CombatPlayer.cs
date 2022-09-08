using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombatPlayer : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private PlayerStats stats;

    [Header("Pooler")]
    [SerializeField] private ObjectPooler pooler1;
    [SerializeField] private ObjectPooler pooler2;

    [Header("Attack")]
    [SerializeField] private Transform[] attackPositions;
    [SerializeField] private GameObject spearWeaponPrefab;

    private int indexAttackDirection;
    private ManaPlayer manaPlayer;
    private float timeToNextAttack1;
    private float timeToNextAttack2;

    // Arma especial (no equipable)
    private float timeToNextSpearAttack;
    private float timeBetweenAttacksSpearWeapon;
    private SpearAttack equippedSpearWeapon;

    public Weapon EquippedWeapon1 { get; private set; }
    public Weapon EquippedWeapon2 { get; private set; }
    public EnemySelection TargetEnemy { get; private set; }

    public bool Attacking { get; set; }

    private void Awake()
    {
        manaPlayer = GetComponent<ManaPlayer>();
        Attacking = false;
    }

    private void Update()
    {

        GetShotDirection();

        if(Time.time > timeToNextAttack1 && !Attacking)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if(EquippedWeapon1 != null)
                {
                    UseWeapon1();
                    timeToNextAttack1 = Time.time + EquippedWeapon1.timeBetweenAttacks;
                }
            }
        }

        if (Time.time > timeToNextAttack2 && !Attacking)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                if (EquippedWeapon2 != null)
                {
                    UseWeapon2();
                    timeToNextAttack2 = Time.time + EquippedWeapon2.timeBetweenAttacks;
                }
            }
        }

        if (Time.time > timeToNextSpearAttack && !Attacking)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (equippedSpearWeapon != null)
                {
                    UseSpearWeapon();
                    timeToNextSpearAttack = Time.time + timeBetweenAttacksSpearWeapon;
                }
            }
        }

    }

    private void UseSpearWeapon()
    {
        StartCoroutine(IESetSpearAttackCondition());
        equippedSpearWeapon.gameObject.SetActive(true);
        Vector3 newPositionAttack = RotateSpearWeapon();
        Vector3 initialPosition = CalculateInitialPositionAttack();
        equippedSpearWeapon.transform.position = initialPosition;
        equippedSpearWeapon.InitializeSpearAttack(initialPosition, newPositionAttack);
        
    }

    public Vector3 actualPositionAttack()
    {
        return attackPositions[indexAttackDirection].position;
    }

    public Vector3 CalculateInitialPositionAttack()
    {
        Vector3 initialPositionAttack = new Vector3(0, 0, 0);
        switch (indexAttackDirection)
        {
            case 0:
                initialPositionAttack = attackPositions[indexAttackDirection].position + new Vector3(0, -0.21f, 0);
                break;
            case 1:
                initialPositionAttack = attackPositions[indexAttackDirection].position + new Vector3(-0.21f, 0, 0);
                break;
            case 2:
                initialPositionAttack = attackPositions[indexAttackDirection].position + new Vector3(0, 0.21f, 0);
                break;
            case 3:
                initialPositionAttack = attackPositions[indexAttackDirection].position + new Vector3(0.21f, 0, 0);
                break;
        }

        return initialPositionAttack;
    }

    public Vector3 RotateSpearWeapon()
    {
        Vector3 newPositionAttack = new Vector3(0,0,0);
        switch (indexAttackDirection)
        {
            case 0:
                equippedSpearWeapon.transform.eulerAngles = new Vector3(0, 0, 180);
                newPositionAttack = attackPositions[indexAttackDirection].position + new Vector3(0, 0.217f, 0);
                break;
            case 1:
                equippedSpearWeapon.transform.eulerAngles = new Vector3(0, 0, 90);
                newPositionAttack = attackPositions[indexAttackDirection].position + new Vector3(0.217f, 0, 0);
                break;
            case 2:
                equippedSpearWeapon.transform.eulerAngles = new Vector3(0, 0, 0);
                newPositionAttack = attackPositions[indexAttackDirection].position + new Vector3(0, -0.217f, 0);
                break;
            case 3:
                equippedSpearWeapon.transform.eulerAngles = new Vector3(0, 0, 270);
                newPositionAttack = attackPositions[indexAttackDirection].position + new Vector3(-0.217f, 0, 0);
                break;
        }

        return newPositionAttack;
    }
    private IEnumerator IESetSpearAttackCondition()
    {
        
        yield return new WaitForSeconds(0.4f);
        equippedSpearWeapon.gameObject.SetActive(false);
    }

    private void UseWeapon1()
    {
        if(EquippedWeapon1.type == WeaponType.Projectile)
        {
            if(manaPlayer.CurrentMana >= EquippedWeapon1.manaRequired)
            {
                StartCoroutine(IESetAttackCondition());

                GameObject newProjectile = pooler1.ObtainInstance();
                newProjectile.transform.localPosition = attackPositions[indexAttackDirection].position;

                Projectile projectile = newProjectile.GetComponent<Projectile>();
                projectile.userType = ProjectileUserType.Player;
                projectile.InitializeProjectile(TargetEnemy, indexAttackDirection, EquippedWeapon1.speed, EquippedWeapon1.ProjectileTimeLife);
                newProjectile.SetActive(true);
                manaPlayer.UseMana(EquippedWeapon1.manaRequired);
            }
        }
 
    }
    private void UseWeapon2()
    {
        if (EquippedWeapon2.type == WeaponType.Projectile)
        {
            if (manaPlayer.CurrentMana >= EquippedWeapon2.manaRequired)
            {
                StartCoroutine(IESetAttackCondition());

                GameObject newProjectile = pooler2.ObtainInstance();
                newProjectile.transform.localPosition = attackPositions[indexAttackDirection].position;
                Projectile projectile = newProjectile.GetComponent<Projectile>();
                projectile.userType = ProjectileUserType.Player;
                projectile.InitializeProjectile(TargetEnemy, indexAttackDirection, EquippedWeapon2.speed, EquippedWeapon2.ProjectileTimeLife);
                newProjectile.SetActive(true);
                manaPlayer.UseMana(EquippedWeapon2.manaRequired);
            }
        }
    }

    public float GetDamage()
    {
        return stats.Damage;
    }

    private IEnumerator IESetAttackCondition()
    {
        Attacking = true;
        yield return new WaitForSeconds(0.3f);
        Attacking = false;
    }

    public void EquipWeapon1(WeaponItem weaponToEquip)
    {
        EquippedWeapon1 = weaponToEquip.Weapon;
        if (weaponToEquip.Weapon.type == WeaponType.Projectile)
        {
            pooler1.CreatePooler(weaponToEquip.Weapon.ProjectilePrefab.gameObject);
        }
        stats.AddBonusForWeapon(weaponToEquip.Weapon);
    }
    public void EquipWeapon2(WeaponItem weaponToEquip)
    {
        EquippedWeapon2 = weaponToEquip.Weapon;
        if (weaponToEquip.Weapon.type == WeaponType.Projectile)
        {
            pooler2.CreatePooler(weaponToEquip.Weapon.ProjectilePrefab.gameObject);
        }
        stats.AddBonusForWeapon(weaponToEquip.Weapon);
    }

    public void EquipSpearWeapon(WeaponItem weaponToEquip)
    {
        timeBetweenAttacksSpearWeapon = weaponToEquip.Weapon.timeBetweenAttacks;
        GameObject go = Instantiate(spearWeaponPrefab, this.gameObject.transform.position, Quaternion.identity);
        equippedSpearWeapon = go.GetComponent<SpearAttack>();
        equippedSpearWeapon.transform.SetParent(this.gameObject.transform);
        equippedSpearWeapon.gameObject.SetActive(false);
        stats.AddBonusForWeapon(weaponToEquip.Weapon);
    }

    public void RemoveWeapon(WeaponItem weaponToRemove)
    {
        
        if (EquippedWeapon1 != null || EquippedWeapon2 != null)
        {
            if (weaponToRemove.Weapon == EquippedWeapon1)
            {
                if (EquippedWeapon1.type == WeaponType.Projectile)
                {
                    pooler1.PoolerDestroy();
                }
                stats.RemoveBonusForWeapon(EquippedWeapon1);
                EquippedWeapon1 = null;
            }
            else if (weaponToRemove.Weapon == EquippedWeapon2)
            {
                if (EquippedWeapon2.type == WeaponType.Projectile)
                {
                    pooler2.PoolerDestroy();
                }
                stats.RemoveBonusForWeapon(EquippedWeapon2);
                EquippedWeapon2 = null;
            }

        }
    }

    private void GetShotDirection()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if(input.x > 0.1f)
        {
            indexAttackDirection = 1; // Se mueve hacia la derecha
        }else if(input.x < 0)
        {
            indexAttackDirection = 3; // Se mueve hacia la izquierda
        }else if(input.y > 0.1f)
        {
            indexAttackDirection = 0; // Se mueve hacia arriba
        }
        else if (input.y < 0)
        {
            indexAttackDirection = 2; // Se mueve hacia abajo
        }
    }

    private void EnemySelectedResponse(EnemySelection selectedEnemy)
    {
        if(TargetEnemy != selectedEnemy)
        {
            TargetEnemy = selectedEnemy;
            TargetEnemy.ShowSelectedEnemy(true);
        }
    }
    private void EnemyDeselectedResponse()
    {
        if(TargetEnemy != null)
        {
            TargetEnemy.ShowSelectedEnemy(false);
            TargetEnemy = null;
        }
    }

    private void OnEnable()
    {
        SelectionManager.EnemySelectedEvent += EnemySelectedResponse;
        SelectionManager.EnemyDeselectedEvent += EnemyDeselectedResponse;
    }

    private void OnDisable()
    {
        SelectionManager.EnemySelectedEvent -= EnemySelectedResponse;
        SelectionManager.EnemyDeselectedEvent -= EnemyDeselectedResponse;
    }

}
