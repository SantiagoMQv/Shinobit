using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Projectile,
    Melee
}

[CreateAssetMenu(menuName = "Player/Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Config")]
    public Sprite WeaponIcon;
    public WeaponType type;
    public float timeBetweenAttacks;

    [Header("Magic Weapon")]
    public Projectile ProjectilePrefab;
    public float manaRequired;
    public float speed;
    public float ProjectileTimeLife;

    [Header("Stats")]
    public int attack;
    public int defense;
}
