// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Animator")]
    private Animator animator;

    [Header("Stat")]
    public string unitName;
    public int unitLevel;
    public int damage;
    public int maxHP;
    public int currentHP;

    private readonly int hashAttack = Animator.StringToHash("Attack");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0) return true;
        else return false;
    }

    public void PlayAttackAnim()
    {
        animator.SetTrigger(hashAttack);
    }
}
