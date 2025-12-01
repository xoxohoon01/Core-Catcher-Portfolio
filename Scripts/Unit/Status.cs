using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Status
{
    public int level;
    public float exp;
    public float maxExp;

    public float maxHP;
    public float hp;
    public float shield;
    public float armor;
    public float damage;
    public float moveSpeed;
    public float attackSpeed;
    public float decreaseCooldown;
    public float skillRange;
    public float skillSpeed;

    public float critChance;
    public float critDamage;

    public Status()
    {

    }
}
