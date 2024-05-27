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
    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    private Unit playerUnit;
    private Unit enemyUnit;

    [Header("Turn")]
    [SerializeField] BattleState curBattleState; // ���� ��Ʋ ��

    [Header("UI")]
    public Text text;

    private void Start()
    {
        curBattleState = BattleState.SelectTurn;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGo = Instantiate(myMonster, playerBattleStation.position, Quaternion.identity); // �÷��̾� ���� ����
        playerGo.transform.rotation = Quaternion.Euler(0, 90, 0);
        playerUnit = playerGo.GetComponent<Unit>(); // �÷��̾� ������ �� ��������
        GameObject enemyGo = Instantiate(enemyMonster, enemyBattleStation.position, Quaternion.identity); // ���� ���� ����
        enemyGo.transform.rotation = Quaternion.Euler(0, -90, 0);
        enemyUnit = enemyGo.GetComponent<Unit>(); // ���� ������ �� ��������

        text.text = "�߻��� " + enemyUnit.unitName + "��(��) ��Ÿ����!";

        yield return new WaitForSeconds(2f); // 2�� ���

        curBattleState = BattleState.MyTurn; // �÷��̾� ������ ��ü
        PlayerTurn(); // �÷��̾� �� ����
    }

    IEnumerator PlayerAttack() // �÷��̾��� ����
    {
        text.text = playerUnit.unitName + "�� ����!";
        playerUnit.PlayAttackAnim();
        playerUnit.EnemyPosSkillAttack(enemyBattleStation);
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage); // ��� ���Ϳ��� �����ϰ� �׾����� Ȯ��
        curBattleState = BattleState.EnemyTurn; // ���� ������ ��ü
        yield return new WaitForSeconds(0.5f);
        text.text = "������ �����ߴ�!";

        yield return new WaitForSeconds(2f); // 2�� ���

        Debug.Log("�÷��̾��� ���� �������ϴ�");

        if (isDead) // ���� �׾��ٸ�
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
        enemyUnit.PlayAttackAnim();
        enemyUnit.EnemyPosSkillAttack(playerBattleStation);
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage); // �÷��̾�� �����ϰ� �׾����� Ȯ��
        yield return new WaitForSeconds(0.5f);
        text.text = "������ �����ߴ�!";

        yield return new WaitForSeconds(2f); // ���

        Debug.Log("����� ���� �������ϴ�");

        if(isDead == true) // �׾��ٸ� 
        {
            text.text = playerUnit.unitName + "��(��) ��������...";
            yield return new WaitForSeconds(1.5f);
            curBattleState = BattleState.Lose; // �÷��̾��� �й�
            EndBattle();
        }
        else
        {
            text.text = "�ൿ�� �����ϼ���!";
            curBattleState = BattleState.MyTurn; // �÷��̾� ������ ��ü
        }
    }

    private void EndBattle() // ��Ʋ ���� �� ��Ʋ ����
    {
        if(curBattleState == BattleState.Won) // �÷��̾ �̰�ٸ�
        {
            text.text = "�߻��� " + enemyUnit.unitName + "�� ���񷶴�!";
        }
        else if(curBattleState == BattleState.Lose) // �÷��̾ ���ٸ�
        {
            text.text = "��ΰ��� ������ įį������";
        }
    }

    private void PlayerTurn() // �÷��̾� �� �ؽ�Ʈ ����
    {
        text.text = "�ൿ�� �����ϼ���!";
    }


    public void OnAttackButton()
    {
        if (curBattleState != BattleState.MyTurn)
            return;

        StartCoroutine(PlayerAttack());
    }
}
