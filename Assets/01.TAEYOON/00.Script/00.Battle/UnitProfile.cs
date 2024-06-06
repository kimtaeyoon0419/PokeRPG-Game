namespace PokeRPG.Battle.Unit
{
    // # System
    using System.Collections;
    using System.Collections.Generic;
    using Unity.VisualScripting;

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
        public string unitName;                                                         // 몬스터 이름
        public int unitLevel;                                                               // 몬스터 레벨
        public float damage;                                                                 // 몬스터 공격력
        public float defence;
        public float maxHP;                                                                   // 몬스터 최대체력
        public float currentHP;                                                             // 몬스터 현재체력
        public int maxExp;                                                                 // 몬스터 레벨업 최대 경험치
        public int curExp;                                                                   // 몬스터 레벨업 현재 경험치
        public int speed;                                                                     // 몬스터 스피드
        public int xpReward;                                                              // 몬스터가 죽고 드랍하는 경험치
        public int evolutionLevel = 15;                                                // 몬스터가 진화하는 레벨
        public type monsterType;                                                            // 몬스터의 타입

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

        public IEnumerator LevelUp(int exp)
        {
            int overflowXP;
            curExp += exp;

            while (curExp >= maxExp)
            {
                yield return new WaitForSeconds(0.5f);
                overflowXP = curExp - maxExp;
                curExp = 0;
                unitLevel++;
                BattleManager.instance.LevelUpText();
                curExp += overflowXP;
            }
            yield return new WaitForSeconds(0.5f);
        }

        public bool ClickLevelUp()
        {
            if (curExp >= maxExp)
            {
                curExp -= maxExp;
                unitLevel++;
                if (unitLevel >= evolutionLevel)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void Evolution()
        {

        }

        public bool TakeDamage(float dmg, UnitProfile attacker, UnitProfile defender)
        {
            animator.SetTrigger(hashTakeDamage);
            currentHP -= Attack.DamageCalc(attacker, defender);

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
                if (skill.skillName == skillName)
                {
                    damage = skill.skillDamage;
                    Instantiate(skill.skillEffet, BattleManager.instance.enemyBattleStation.position, Quaternion.identity);
                    break;
                }
            }
        }
    }
}
