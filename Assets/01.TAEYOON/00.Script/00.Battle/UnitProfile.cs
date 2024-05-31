namespace PokeRPG.Battle.Unit
{
    // # System
    using System.Collections;
    using System.Collections.Generic;

    // # Unity
    using UnityEngine;

    [System.Serializable]
    public class SkillList
    {
        public string skillName;

        public GameObject skillEffet;

        public int skillDamage;
    }


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
        public int exp;

        [Header("SkillEffect")]
        public GameObject tonadoEffect;
        public List<SkillList> skillList;

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

        public void UseSkill(string skillName)
        {
            foreach (var skill in skillList)
            {
                if(skill.skillName == skillName)
                {
                    damage = skill.skillDamage;
                    Instantiate(skill.skillEffet, BattleManager.instance.enemyBattleStation.position, Quaternion.identity);
                    break;
                }
            }
        }
    }
}
