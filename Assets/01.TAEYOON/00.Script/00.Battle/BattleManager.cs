namespace PokeRPG.Battle
{
    using PokeRPG.Battle.UI;
    using PokeRPG.Battle.Unit;
    // # System
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Unity.VisualScripting;

    // # Unity
    using UnityEngine;
    using UnityEngine.UI;

    public class BattleManager : MonoBehaviour
    {
        public static BattleManager instance; // �̱���

        [Header("Monsters")]
        public GameObject myMonster; // �� ����
        public GameObject enemyMonster; // ��� ����
        public Transform playerBattleStation; // �÷��̾� ���� ������ġ
        public Transform enemyBattleStation; // ���� ���� ������ġ

        public UnitProfile playerUnit { get; private set; }// �÷��̾� ������ UnitProfile �ڵ�
        public UnitProfile enemyUnit { get; private set; }// ���� ������ UnitProfile �ڵ�

        public BattleState curBattleState { get; private set; } // ���� ��Ʋ ��

        [Header("UI")]
        public Text text; // ��Ʋ ��Ȳ�� �˷��� �ؽ�Ʈ
        public GameObject skillButton1; // ��ų ��ư
        public GameObject playerMonsterStatBoxObject;
        public GameObject enemyMonsterStatBoxObject;
        private MonsterStatBox playerMonsterStatBox;
        private MonsterStatBox enemyMonsterStatBox;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            curBattleState = BattleState.SelectTurn; // ���� ��Ʋ���� �ൿ ���������� ����
            StartCoroutine(SetupBattle()); // �� �⺻���� 
            skillButton1.SetActive(false); // ��ų ��ư ��Ȱ��ȭ
        }

        IEnumerator SetupBattle()
        {
            GameObject playerGo = Instantiate(myMonster, playerBattleStation.position, Quaternion.identity); // �÷��̾� ���� ����
            playerGo.transform.rotation = Quaternion.Euler(0, 90, 0); // ���� �ٶ󺸱�
            playerUnit = playerGo.GetComponent<UnitProfile>(); // �÷��̾� ������ �� ��������
            GameObject enemyGo = Instantiate(enemyMonster, enemyBattleStation.position, Quaternion.identity); // ���� ���� ����
            enemyGo.transform.rotation = Quaternion.Euler(0, -90, 0); // ���� �ٶ󺸱�
            enemyUnit = enemyGo.GetComponent<UnitProfile>(); // ���� ������ �� ��������

            playerMonsterStatBox = playerMonsterStatBoxObject.GetComponent<MonsterStatBox>(); // �÷��̾� ���� ���ݹڽ� ��������

            text.text = "�߻��� " + enemyUnit.unitName + "��(��) ��Ÿ����!";

            yield return new WaitForSeconds(2f); // ���

            curBattleState = BattleState.MyTurn; // �÷��̾� ������ ��ü
            PlayerTurn(); // �÷��̾� �� ����
        }

        IEnumerator PlayerAttack() // �÷��̾��� ����
        {
            text.text = playerUnit.unitName + "�� ����!";
            playerUnit.PlayAttackAnim(); // �÷��̾� ������ ���� �ִϸ��̼� ����
            playerUnit.EnemyPosSkillAttack(enemyBattleStation); // ��ų ����Ʈ ��ȯ
            bool isDead = enemyUnit.TakeDamage(playerUnit.damage); // ��� ���Ϳ��� �����ϰ� �׾����� Ȯ��
            curBattleState = BattleState.EnemyTurn; // ���� ������ ��ü
            yield return new WaitForSeconds(0.5f); // ���
            text.text = "������ �����ߴ�!";

            yield return new WaitForSeconds(2f); // ���

            Debug.Log("�÷��̾��� ���� �������ϴ�");

            if (isDead) // ���� ��� ���Ͱ� �׾��ٸ�
            {
                curBattleState = BattleState.Won; // �÷��̾� �¸� 
                EndBattle(); // ��Ʋ ����
            }
            else // ���׾��ٸ�
            {
                StartCoroutine(EnemyTurn()); // ���� �� ����
            }
        }

        IEnumerator EnemyTurn() // ������ ����
        {
            yield return new WaitForSeconds(1f); // ���

            text.text = enemyUnit.unitName + "�� ����!";
            enemyUnit.PlayAttackAnim(); // ���� ������ ���� �ִϸ��̼� ����
            enemyUnit.EnemyPosSkillAttack(playerBattleStation); // ���� ������ ��ų ����Ʈ ��ȯ
            bool isDead = playerUnit.TakeDamage(enemyUnit.damage); // �÷��̾�� �����ϰ� �׾����� Ȯ��
            yield return new WaitForSeconds(0.5f); // ���
            text.text = "������ �����ߴ�!";
            //playerMonsterStatBox.SetStatBox();

            yield return new WaitForSeconds(2f); // ���

            Debug.Log("����� ���� �������ϴ�");

            if (isDead == true) // ���� �÷��̾� ���Ͱ� �׾��ٸ� 
            {
                text.text = playerUnit.unitName + "��(��) ��������...";
                yield return new WaitForSeconds(1.5f); // ���
                curBattleState = BattleState.Lose; // �÷��̾��� �й�
                EndBattle(); // ��Ʋ ����
            }
            else
            {
                PlayerTurn(); // �÷��̾� �ൿ ����â
                curBattleState = BattleState.MyTurn; // �÷��̾� ������ ��ü
            }
        }

        private void EndBattle() // ��Ʋ ���� �� ��Ʋ ����
        {
            if (curBattleState == BattleState.Won) // �÷��̾ �̰�ٸ�
            {
                text.text = "�߻��� " + enemyUnit.unitName + "�� ���񷶴�!";
            }
            else if (curBattleState == BattleState.Lose) // �÷��̾ ���ٸ�
            {
                text.text = "��ΰ��� ������ įį������";
            }
        }

        private void PlayerTurn() // �÷��̾� �� �ؽ�Ʈ ����
        {
            text.text = "�ൿ�� �����ϼ���!";
            skillButton1.SetActive(true); // ��ų ��ư Ȱ��ȭ
        }


        public void OnAttackButton() // ��ư Ŭ������ �� ������ �Լ�
        {
            if (curBattleState != BattleState.MyTurn) // �÷��̾� ���� �ƴ϶�� �׳� ����
                return;

            skillButton1.SetActive (false); // ��ų ��ư ��Ȱ��ȭ
            StartCoroutine(PlayerAttack()); // �÷��̾� ������ ���� �ڷ�ƾ ����
        }
    }
}
