namespace PokeRPG.Battle.Unit
{
    // # System
    using System.Collections;
    using System.Collections.Generic;

    // # Unity
    using UnityEngine;

    public class UnitProfile : MonoBehaviour
    {
        [Header("Animator")]
        private Animator animator;

        [Header("Stat")]
        public string unitName;
        public int unitLevel;
        public int damage;
        public int maxHP;
        public int currentHP;
        public int maxExp;
        public int curExp;
        public int speed;

        [Header("SkillEffect")]
        public GameObject tonadoEffect;

        private readonly int hashAttack = Animator.StringToHash("Attack");
        private readonly int hashTakeDamage = Animator.StringToHash("TakeDamage");
        private readonly int hashDie = Animator.StringToHash("Die");

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public bool TakeDamage(int dmg)
        {
            animator.SetTrigger(hashTakeDamage);
            currentHP -= dmg;

            if (currentHP <= 0)
            {
                animator.SetTrigger(hashDie);
                return true;
            }
            else return false;
        }

        public void PlayAttackAnim()
        {
            animator.SetTrigger(hashAttack);
        }

        public void EnemyPosSkillAttack(Transform skillpos)
        {
            Instantiate(tonadoEffect, skillpos.position, Quaternion.identity);
        }
    }
}
