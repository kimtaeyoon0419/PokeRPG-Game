namespace PokeRPG.Battle.UI
{
    // # System
    using System.Collections;
    using System.Collections.Generic;

    // # Unity
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class MonsterStatBox : MonoBehaviour
    {
        [Header("UI")]
        public TextMeshProUGUI monsterName; // 몬스터의 이름
        public TextMeshProUGUI monsterHP; // 몬스터의 체력 텍스트
        public TextMeshProUGUI monsterLevel; // 몬스터의 레벨 텍스트
        public Slider monsterHPbar; // 몬스터의 체력바
        public Slider easeHpBar;
        public Slider monsterExpbar; // 몬스터의 경험치바
        public Gradient gradient; // 그레디언트
        public Image fill; // 슬라이더 바 이미지
        public WhoMonster whomonster;
        private float lerpspeed = 0.01f;

        private void Start()
        {
            if (whomonster == WhoMonster.myMoster)
            {
                monsterHPbar.value = (float)BattleManager.instance.playerUnit.currentHP / BattleManager.instance.playerUnit.maxHP;
                monsterExpbar.value = (float)BattleManager.instance.playerUnit.curExp / BattleManager.instance.playerUnit.maxExp;
            }
        }

        private void LateUpdate()
        {
            SetStatBox();
        }

        private void SetStatBox()
        {
            if (whomonster == WhoMonster.myMoster)
            {
                monsterName.text = BattleManager.instance.playerUnit.unitName;

                monsterHP.text = BattleManager.instance.playerUnit.currentHP.ToString() + " / " + BattleManager.instance.playerUnit.maxHP.ToString(); // 체력 초기화 ( 텍스트 )

                monsterLevel.text = "Lv." + BattleManager.instance.playerUnit.unitLevel.ToString();

                monsterHPbar.value = Mathf.Lerp(monsterHPbar.value, (float)BattleManager.instance.playerUnit.currentHP / BattleManager.instance.playerUnit.maxHP, lerpspeed); // 체력바 초기화

                if (monsterExpbar.value <= (float)BattleManager.instance.playerUnit.curExp / BattleManager.instance.playerUnit.maxExp)
                {
                    monsterExpbar.value = Mathf.Lerp(monsterExpbar.value, (float)BattleManager.instance.playerUnit.curExp / BattleManager.instance.playerUnit.maxExp, lerpspeed); // 경험치바 초기화
                }
                else
                {
                    monsterExpbar.value = 0;
                    monsterExpbar.value = Mathf.Lerp(monsterExpbar.value, (float)BattleManager.instance.playerUnit.curExp / BattleManager.instance.playerUnit.maxExp, lerpspeed); // 경험치바 초기화
                }

                fill.color = gradient.Evaluate(monsterHPbar.normalizedValue);
            }
            else
            {
                monsterName.text = BattleManager.instance.enemyUnit.unitName;

                monsterHPbar.value = Mathf.Lerp(monsterHPbar.value, (float)BattleManager.instance.enemyUnit.currentHP / BattleManager.instance.enemyUnit.maxHP, lerpspeed); // 체력바 초기화

                fill.color = gradient.Evaluate(monsterHPbar.normalizedValue);
            }
        }

        private void LevelUp()
        {
            monsterLevel.text = "Lv" + BattleManager.instance.playerUnit.unitLevel.ToString(); // 레벨 초기화
            monsterExpbar.value = BattleManager.instance.playerUnit.maxExp; // 경험치바 초기화
        }
    }
}