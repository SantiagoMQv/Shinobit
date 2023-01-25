using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class CombatPlayer : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private PlayerStats stats;

    [Header("Pooler")]
    [SerializeField] private ObjectPooler pooler1;
    [SerializeField] private ObjectPooler pooler2;
    [SerializeField] private ObjectPooler shurikenPooler;

    [Header("Attack")]
    [SerializeField] private Transform[] attackPositions;
    [SerializeField] private GameObject spearWeaponPrefab;
    [SerializeField] private GameObject shieldNinjutsuVFXPrefab;

    public static Action<string, Color> FloatingTextCountdownEvent;

    private int indexAttackDirection;
    private ManaPlayer manaPlayer;
    private float timeToNextAttack1;
    private float timeToNextAttack2;

    // Arma especial: Lanza (no equipable)
    private float timeToNextSpearAttack;
    private float timeBetweenAttacksSpearWeapon;
    private SpearAttack equippedSpearWeapon;

    // Arma especial: Shuriken (no equipable)
    private float timeToNextShurikenAttack;
    private Weapon equippedShurikenWeapon;
    private Player player;

    // Arma especial: Escudo (no equipable)
    private ShieldNinjutsuItem shieldNinjutsuItem;
    private float timeToNextShieldNinjutsu;
    private bool shieldNinjutsuCountdownDone;
    private GameObject shieldNinjutsuVFX;

    // Arma especial: Curación (no equipable)
    private HealingNinjutsuItem healingNinjutsuItem;
    private int NumTokens;
    private float HPRestoration;

    public Weapon EquippedWeapon1 { get; private set; }
    public Weapon EquippedWeapon2 { get; private set; }
    public EnemySelection TargetEnemy { get; private set; }
    public bool Attacking { get; private set; }
    public bool Healing { get; private set; }
    public GameObject ShieldNinjutsuVFX => shieldNinjutsuVFX;

    private void Awake()
    {
        player = Player.Instance;
        manaPlayer = GetComponent<ManaPlayer>();
        Attacking = false;
        Healing = false;
    }

    private void Update()
    {
        if (!player.playerJump.Jumping && !Attacking)
        {
            AttackSlot1();
            AttackSlot2();
            AttackShuriken();
            AttackSpear();
            UseShield();
            UseHealingNinjutsu();
        }

        CountdownShieldNinjutsu();

        GetShotDirection();
    }

    #region Attacks And Uses

    private void AttackSlot1()
    {
        if (Time.time > timeToNextAttack1 )
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (EquippedWeapon1 != null)
                {
                    UseWeapon1();
                    timeToNextAttack1 = Time.time + EquippedWeapon1.timeBetweenAttacks;
                }
            }
        }
    }

    private void AttackSlot2()
    {
        if (Time.time > timeToNextAttack2 )
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
    }
    private void AttackShuriken()
    {
        if (Time.time > timeToNextShurikenAttack )
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (equippedShurikenWeapon != null)
                {
                    UseShurikenWeapon();
                    timeToNextSpearAttack = Time.time + equippedShurikenWeapon.timeBetweenAttacks;
                }
            }
        }
    }

    private void AttackSpear()
    {
        if (Time.time > timeToNextSpearAttack)
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
    private void UseShield()
    {
        if (Time.time > timeToNextShieldNinjutsu )
        {
            if (Input.GetKeyDown(KeyCode.N) && !Healing && !player.playerJump.Jumping && !player.HealthPlayer.Defeated)
            {
                if (shieldNinjutsuItem != null)
                {
                    shieldNinjutsuItem.UseItem();
                    shieldNinjutsuVFX.SetActive(true);
                    timeToNextShieldNinjutsu = Time.time + shieldNinjutsuItem.reuseTime;
                    shieldNinjutsuCountdownDone = false;
                }
            }
        }
    }
    private void UseHealingNinjutsu()
    {
        if (healingNinjutsuItem != null)
        {
            if (Input.GetKeyDown(KeyCode.P) && !Healing  && !player.HealthPlayer.Defeated && NumTokens > 0 && player.HealthPlayer.CanBeHealed )
            {
                healingNinjutsuItem.UseItem();
            }
        }
    }

    #endregion

    private void CountdownShieldNinjutsu()
    {
        if (shieldNinjutsuItem != null)
        {
            if (timeToNextShieldNinjutsu - Time.time <= 3 && !shieldNinjutsuCountdownDone)
            {
                StartCoroutine(IEDeactivateShieldNinjutsu());
                FloatingTextCountdownEvent?.Invoke("Escudo", new Color32(51, 21, 108, 255));
                shieldNinjutsuCountdownDone = true;
            }
        }
    }

    private IEnumerator IEDeactivateShieldNinjutsu()
    {
        yield return new WaitForSeconds(3);
        ShieldHealthPlayer shieldHealthPlayer = GetComponent<ShieldHealthPlayer>();
        shieldHealthPlayer.ShieldHealthBar.gameObject.transform.parent.gameObject.SetActive(false);
        shieldHealthPlayer.enabled = false;
        shieldNinjutsuVFX.SetActive(false);
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
        Attacking = true;
        yield return new WaitForSeconds(0.4f);
        equippedSpearWeapon.gameObject.SetActive(false);
        Attacking = false;
    }

    private void UseWeapon1()
    {
        if(EquippedWeapon1.type == WeaponType.Projectile)
        {
            if(manaPlayer.CurrentMana >= EquippedWeapon1.manaRequired)
            {
                StartCoroutine(IESetAttackCondition(EquippedWeapon1.timeBetweenAttacks));

                GameObject newProjectile = pooler1.ObtainInstance();
                newProjectile.transform.localPosition = attackPositions[indexAttackDirection].position;

                Projectile projectile = newProjectile.GetComponent<Projectile>();
                projectile.userType = ProjectileUserType.Player;
                projectile.InitializeProjectile(TargetEnemy, indexAttackDirection, EquippedWeapon1.speed, EquippedWeapon1.ProjectileTimeLife, ProjectileUserType.Player);
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
                StartCoroutine(IESetAttackCondition(EquippedWeapon2.timeBetweenAttacks));

                GameObject newProjectile = pooler2.ObtainInstance();
                newProjectile.transform.localPosition = attackPositions[indexAttackDirection].position;
                Projectile projectile = newProjectile.GetComponent<Projectile>();
                projectile.InitializeProjectile(TargetEnemy, indexAttackDirection, EquippedWeapon2.speed, EquippedWeapon2.ProjectileTimeLife, ProjectileUserType.Player);
                newProjectile.SetActive(true);
                manaPlayer.UseMana(EquippedWeapon2.manaRequired);
            }
        }
    }
    private void UseShurikenWeapon()
    {
        if (equippedShurikenWeapon != null)
        {
            if (manaPlayer.CurrentMana >= equippedShurikenWeapon.manaRequired)
            {
                StartCoroutine(IESetAttackCondition(equippedShurikenWeapon.timeBetweenAttacks));

                GameObject newProjectile = shurikenPooler.ObtainInstance();
                newProjectile.transform.localPosition = attackPositions[indexAttackDirection].position;
                Projectile projectile = newProjectile.GetComponent<Projectile>();
                projectile.InitializeProjectile(TargetEnemy, indexAttackDirection, equippedShurikenWeapon.speed, equippedShurikenWeapon.ProjectileTimeLife, ProjectileUserType.Player);
                newProjectile.SetActive(true);
                manaPlayer.UseMana(equippedShurikenWeapon.manaRequired);
            }
        }
    }

    #region HealingNinjutsu

    public float GetHpHPRestoration()
    {
        return HPRestoration;
    }

    public void RemoveHealthToken()
    {
        if (NumTokens > 0)
        {
            NumTokens = UIManager.Instance.RemoveHealthTokenUI() - 1;
        }
    }

    public void AddAllHealthToken()
    {
        if (healingNinjutsuItem != null)
        {
            if (NumTokens < 8)
            {

                for (int i = NumTokens; i < stats.Potion; i++)
                {
                    NumTokens = UIManager.Instance.AddHealthTokenUI();
                }

            }
        }
    }

    IEnumerator HealWaiting()
    {
        Healing = true;
        yield return new WaitForSeconds(1f);
        Healing = false;
    }

    public void setHealing(bool state)
    {
        Healing = state;
    }

    #endregion
    public float GetDamage()
    {
        return stats.Damage;
    }

    private IEnumerator IESetAttackCondition(float waitTime)
    {
        Attacking = true;
        yield return new WaitForSeconds(waitTime);
        Attacking = false;
    }

    #region Equip

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

    public void EquipShurikenWeapon(WeaponItem weaponToEquip)
    {
        equippedShurikenWeapon = weaponToEquip.Weapon;
        shurikenPooler.CreatePooler(weaponToEquip.Weapon.ProjectilePrefab.gameObject);
        stats.AddBonusForWeapon(weaponToEquip.Weapon);
    }

    public void EquipShieldNinjutsu(ShieldNinjutsuItem shield)
    {
        shieldNinjutsuItem = shield;
        shieldNinjutsuVFX = Instantiate(shieldNinjutsuVFXPrefab, transform.position, Quaternion.identity);
        shieldNinjutsuVFX.transform.SetParent(transform);
        shieldNinjutsuVFX.SetActive(false);
        shieldNinjutsuCountdownDone = true;
    }
    public void EquipHealingNinjutsu(HealingNinjutsuItem healingNinjutsu)
    {
        healingNinjutsuItem = healingNinjutsu;
        NumTokens = 0;
        HPRestoration = healingNinjutsu.InitialHPRestoration;
        UIManager.Instance.GenerateHealthTokenPanel();
        AddAllHealthToken();
    }
    #endregion

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
