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
    using TMPro;
    using UnityEngine.SceneManagement;
    using PokeRPG.Sound;
    using UnityEditor.Rendering;

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
        public bool playerWin;
        public bool gameEnd;

        [Header("UI")]
        public TextMeshProUGUI text;
        public GameObject skillButton1; // ��ų ��ư
        public GameObject skillButton2; // ��ų ��ư
        public GameObject skillButton3; // ��ų ��ư
        public GameObject skillButton4; // ��ų ��ư
        public GameObject playerMonsterStatBoxObject; // ȭ�鿡 ǥ�õǴ� ����â ( �÷��̾� )
        public GameObject enemyMonsterStatBoxObject; // ȭ�鿡 ǥ�õǴ� ����â ( ��� )
        public Image FadePanel;
        private float clickTextdelay = 0.2f;

        

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Update()
        {
            if (playerWin)
            {
                if(Input.GetMouseButtonDown(0) && clickTextdelay <= 0)
                {
                    WinText();
                    clickTextdelay = 0.2f;
                }
            }
            if(clickTextdelay >= 0)
            {
                clickTextdelay -= Time.deltaTime;
            }
        }

        private void Start()
        {
            curBattleState = BattleState.SelectTurn; // ���� ��Ʋ���� �ൿ ���������� ����
            StartCoroutine(SetupBattle()); // �� �⺻���� 
            skillButton1.SetActive(false); // ��ų ��ư ��Ȱ��ȭ
            skillButton2.SetActive(false); // ��ų ��ư ��Ȱ��ȭ
            skillButton3.SetActive(false); // ��ų ��ư ��Ȱ��ȭ
            skillButton4.SetActive(false); // ��ų ��ư ��Ȱ��ȭ
        }

        IEnumerator SetupBattle()
        {
            GameObject playerGo = Instantiate(myMonster, playerBattleStation.position, Quaternion.identity); // �÷��̾� ���� ����
            playerGo.transform.rotation = Quaternion.Euler(0, 90, 0); // ���� �ٶ󺸱�
            playerUnit = playerGo.GetComponent<UnitProfile>(); // �÷��̾� ������ �� ��������
            GameObject enemyGo = Instantiate(enemyMonster, enemyBattleStation.position, Quaternion.identity); // ���� ���� ����
            enemyGo.transform.rotation = Quaternion.Euler(0, -81, 0); // ���� �ٶ󺸱�
            enemyUnit = enemyGo.GetComponent<UnitProfile>(); // ���� ������ �� ��������
            SoundManager.instance.PlayMusic("BattleBGM_1");

            text.text = "�߻��� " + enemyUnit.unitName + "��(��) ��Ÿ����!";

            yield return new WaitForSeconds(2f); // ���

            if (playerUnit.speed > enemyUnit.speed)
            {
                curBattleState = BattleState.MyTurn; // �÷��̾� ������ ��ü
                PlayerTurn(); // �÷��̾� �� ����
            }
            else
            {
                StartCoroutine(EnemyTurn());
            }
        }

        IEnumerator PlayerAttack() // �÷��̾��� ����
        {
            text.text = playerUnit.unitName + "�� ����!";
            playerUnit.PlayAttackAnim(); // �÷��̾� ������ ���� �ִϸ��̼� ����
            //playerUnit.EnemyPosSkillAttack(enemyBattleStation); // ��ų ����Ʈ ��ȯ
            bool isDead = enemyUnit.TakeDamage(playerUnit.damage); // ��� ���Ϳ��� �����ϰ� �׾����� Ȯ��
            curBattleState = BattleState.EnemyTurn; // ���� ������ ��ü
            yield return new WaitForSeconds(0.5f); // ���
            text.text = "������ �����ߴ�!";

            yield return new WaitForSeconds(2f); // ���

            Debug.Log("�÷��̾��� ���� �������ϴ�");

            if (isDead) // ���� ��� ���Ͱ� �׾��ٸ�
            {
                SoundManager.instance.PlayMusic("Victory");
                text.text = playerUnit.unitName + "��(��) " + enemyUnit.xpReward + " ����ġ�� �����!";
                yield return new WaitForSeconds(0.5f);
                playerWin = true;
                curBattleState = BattleState.Won; // �÷��̾� �¸�
                //yield return playerUnit.StartCoroutine(playerUnit.LevelUp(enemyUnit.xpReward));
                playerUnit.curExp += enemyUnit.xpReward;

                //if (playerUnit.curExp <= playerUnit.maxExp)
                //{
                //    EndBattle(); // ��Ʋ ����
                //}
            }
            else // ���׾��ٸ�
            {
                StartCoroutine(EnemyTurn()); // ���� �� ����
            }
        }

        IEnumerator EnemyTurn() // ������ ����
        {
            //yield return new WaitForSeconds(1f); // ���

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

        private void WinText()
        {
            if(playerUnit.curExp >= playerUnit.maxExp)
            {
                playerUnit.ClickLevelUp();
                LevelUpText();
                StartCoroutine(SoundManager.instance.PlaySfx("LevelUp!"));
            }
            else
            {
                if (gameEnd)
                {
                    StartCoroutine(Co_Fade());
                    playerWin = false;
                }
                else
                {
                    text.text = "�߻��� " + enemyUnit.unitName + "�� ���񷶴�!";
                    gameEnd = true;
                }
            }
        }

        private void EndBattle() // ��Ʋ ���� �� ��Ʋ ����
        {
            if (curBattleState == BattleState.Won) // �÷��̾ �̰�ٸ�
            {
                text.text = "�߻��� " + enemyUnit.unitName + "�� ���񷶴�!";
                StartCoroutine(Co_Fade());
            }
            else if (curBattleState == BattleState.Lose) // �÷��̾ ���ٸ�
            {
                text.text = "��ΰ��� ������ įį������";
                StartCoroutine(Co_Fade());
            }
        }

        private void PlayerTurn() // �÷��̾� �� �ؽ�Ʈ ����
        {
            text.text = "�ൿ�� �����ϼ���!";
            skillButton1.SetActive(true); // ��ų ��ư Ȱ��ȭ
            skillButton2.SetActive(true); // ��ų ��ư Ȱ��ȭ
            skillButton3.SetActive(true); // ��ų ��ư Ȱ��ȭ
            skillButton4.SetActive(true); // ��ų ��ư Ȱ��ȭ
        }


        public void OnAttackButton(string skillName) // ��ư Ŭ������ �� ������ �Լ�
        {
            if (curBattleState != BattleState.MyTurn) // �÷��̾� ���� �ƴ϶�� �׳� ����
                return;

            skillButton1.SetActive(false); // ��ų ��ư ��Ȱ��ȭ
            skillButton2.SetActive(false); // ��ų ��ư ��Ȱ��ȭ
            skillButton3.SetActive(false); // ��ų ��ư ��Ȱ��ȭ
            skillButton4.SetActive(false); // ��ų ��ư ��Ȱ��ȭ
            playerUnit.UseSkill(skillName);
            StartCoroutine(PlayerAttack()); // �÷��̾� ������ ���� �ڷ�ƾ ����
        }

        public void LevelUpText()
        {
            text.text = playerUnit.unitName + "�� ���� " + playerUnit.unitLevel + "�� �ö���!";
        }

        private IEnumerator Co_Fade()
        {
            Color color = FadePanel.color;

            while (color.a < 1)
            {
                color.a += Time.deltaTime;
                yield return null;
                FadePanel.color = color;
            }
            SceneChange();
        }

        private void SceneChange()
        {
            if(playerUnit.unitLevel >= playerUnit.evolutionLevel)
            {
                SceneManager.LoadScene("EvolutionScene");
            }
            else
            {
                
            }
        }
    }
}