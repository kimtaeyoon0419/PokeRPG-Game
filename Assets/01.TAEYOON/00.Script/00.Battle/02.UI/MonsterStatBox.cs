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
        public Text monsterName; // ������ �̸�
        public Text monsterHP; // ������ ü�� �ؽ�Ʈ
        public Text monsterLevel; // ������ ���� �ؽ�Ʈ
        public Slider monsterHPbar; // ������ ü�¹�
        public Slider easeHpBar;
        public Slider monsterExpbar; // ������ ����ġ��
        public Gradient gradient; // �׷����Ʈ
        public Image fill; // �����̴� �� �̹���
        public WhoMonster whomonster;
        private float lerpspeed = 0.01f;

        private void Start()
        {
            SetStatBox();
        }

        private void Update()
        {
            SeeDamage();
        }

        public void SetStatBox()
        {
            StartCoroutine(Co_SetStatBox());
        }

        private void SeeDamage()
        {
            if (whomonster == WhoMonster.myMoster)
            {
                monsterHP.text = BattleManager.instance.playerUnit.currentHP.ToString() + " / " + BattleManager.instance.playerUnit.maxHP.ToString(); // ü�� �ʱ�ȭ ( �ؽ�Ʈ )

                monsterHPbar.value = Mathf.Lerp(monsterHPbar.value, (float)BattleManager.instance.playerUnit.currentHP / BattleManager.instance.playerUnit.maxHP, lerpspeed); // ü�¹� �ʱ�ȭ

                fill.color = gradient.Evaluate(monsterHPbar.normalizedValue);
            }
            else
            {
                monsterHPbar.value = Mathf.Lerp(monsterHPbar.value, (float)BattleManager.instance.enemyUnit.currentHP / BattleManager.instance.enemyUnit.maxHP, lerpspeed); // ü�¹� �ʱ�ȭ

                fill.color = gradient.Evaluate(monsterHPbar.normalizedValue);
            }
        }

        private IEnumerator Co_SetStatBox()
        {
            yield return null;
            if (whomonster == WhoMonster.myMoster)
            {
                monsterName.text = BattleManager.instance.playerUnit.unitName; // �̸� �ʱ�ȭ 
                monsterHP.text = BattleManager.instance.playerUnit.currentHP.ToString() + " / " + BattleManager.instance.playerUnit.maxHP.ToString(); // ü�� �ʱ�ȭ
                monsterLevel.text = "Lv" + BattleManager.instance.playerUnit.unitLevel.ToString(); // ���� �ʱ�ȭ
                monsterExpbar.value = BattleManager.instance.playerUnit.maxExp; // ����ġ�� �ʱ�ȭ
                fill.color = gradient.Evaluate(monsterHPbar.normalizedValue);
            }
            else
            {
                monsterName.text = BattleManager.instance.playerUnit.unitName; // �̸� �ʱ�ȭ 
                fill.color = gradient.Evaluate(monsterHPbar.normalizedValue);
            }
        }
    }
}