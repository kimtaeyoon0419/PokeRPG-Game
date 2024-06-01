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
        public TextMeshProUGUI monsterName; // ������ �̸�
        public TextMeshProUGUI monsterHP; // ������ ü�� �ؽ�Ʈ
        public TextMeshProUGUI monsterLevel; // ������ ���� �ؽ�Ʈ
        public Slider monsterHPbar; // ������ ü�¹�
        public Slider easeHpBar;
        public Slider monsterExpbar; // ������ ����ġ��
        public Gradient gradient; // �׷����Ʈ
        public Image fill; // �����̴� �� �̹���
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

                monsterHP.text = BattleManager.instance.playerUnit.currentHP.ToString() + " / " + BattleManager.instance.playerUnit.maxHP.ToString(); // ü�� �ʱ�ȭ ( �ؽ�Ʈ )

                monsterLevel.text = "Lv." + BattleManager.instance.playerUnit.unitLevel.ToString();

                monsterHPbar.value = Mathf.Lerp(monsterHPbar.value, (float)BattleManager.instance.playerUnit.currentHP / BattleManager.instance.playerUnit.maxHP, lerpspeed); // ü�¹� �ʱ�ȭ

                if (monsterExpbar.value <= (float)BattleManager.instance.playerUnit.curExp / BattleManager.instance.playerUnit.maxExp)
                {
                    monsterExpbar.value = Mathf.Lerp(monsterExpbar.value, (float)BattleManager.instance.playerUnit.curExp / BattleManager.instance.playerUnit.maxExp, lerpspeed); // ����ġ�� �ʱ�ȭ
                }
                else
                {
                    monsterExpbar.value = 0;
                    monsterExpbar.value = Mathf.Lerp(monsterExpbar.value, (float)BattleManager.instance.playerUnit.curExp / BattleManager.instance.playerUnit.maxExp, lerpspeed); // ����ġ�� �ʱ�ȭ
                }

                fill.color = gradient.Evaluate(monsterHPbar.normalizedValue);
            }
            else
            {
                monsterName.text = BattleManager.instance.enemyUnit.unitName;

                monsterHPbar.value = Mathf.Lerp(monsterHPbar.value, (float)BattleManager.instance.enemyUnit.currentHP / BattleManager.instance.enemyUnit.maxHP, lerpspeed); // ü�¹� �ʱ�ȭ

                fill.color = gradient.Evaluate(monsterHPbar.normalizedValue);
            }
        }

        private void LevelUp()
        {
            monsterLevel.text = "Lv" + BattleManager.instance.playerUnit.unitLevel.ToString(); // ���� �ʱ�ȭ
            monsterExpbar.value = BattleManager.instance.playerUnit.maxExp; // ����ġ�� �ʱ�ȭ
        }
    }
}