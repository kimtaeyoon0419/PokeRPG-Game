namespace PokeRPG.Battle.UI
{
    // # System
    using System.Collections;
    using System.Collections.Generic;

    // # Unity
    using UnityEngine;
    using UnityEngine.UI;

    public class MonsterStatBox : MonoBehaviour
    {
        [Header("UI")]
        public Text monsterName; // 몬스터의 이름
        public Text monsterHP; // 몬스터의 체력 텍스트
        public Text monsterLevel; // 몬스터의 레벨 텍스트
        public Slider monsterHPbar; // 몬스터의 체력바
        public Slider easeHpBar;
        public Slider monsterExpbar; // 몬스터의 경험치바
        public Gradient gradient; // 그레디언트
        public Image fill; // 슬라이더 바 이미지
        public WhoMonster whomonster;
        private float lerpspeed = 0.01f;


        private void Update()
        {
            SetStatBox();
        }

        private void SetStatBox()
        {
            if (whomonster == WhoMonster.myMoster)
            {
                monsterHP.text = BattleManager.instance.playerUnit.currentHP.ToString() + " / " + BattleManager.instance.playerUnit.maxHP.ToString(); // 체력 초기화 ( 텍스트 )

                monsterHPbar.value = Mathf.Lerp(monsterHPbar.value, (float)BattleManager.instance.playerUnit.currentHP / BattleManager.instance.playerUnit.maxHP, lerpspeed); // 체력바 초기화

                fill.color = gradient.Evaluate(monsterHPbar.normalizedValue);
            }
            else
            {
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