// # System
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

// # Unity
using UnityEngine;

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

    private void Start()
    {
        curBattleState = BattleState.SelectTurn;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGo = Instantiate(myMonster, playerBattleStation); // �÷��̾� ���� ����
        playerUnit = playerGo.GetComponent<Unit>(); // �÷��̾� ������ �� ��������
        GameObject enemyGo = Instantiate(enemyMonster, enemyBattleStation); // ���� ���� ����
        enemyUnit = enemyGo.GetComponent<Unit>(); // ���� ������ �� ��������

        yield return new WaitForSeconds(2f); // 2�� ���

        curBattleState = BattleState.MyTurn; // �÷��̾� ������ ��ü
        PlayerTurn(); // �÷��̾� �� ����
    }

    IEnumerator PlayerAttack() // �÷��̾��� ����
    {
        playerUnit.PlayAttackAnim();
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage); // ��� ���Ϳ��� �����ϰ� �׾����� Ȯ��
        curBattleState = BattleState.EnemyTurn; // ���� ������ ��ü

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
        yield return new WaitForSeconds(2f); // ���

        enemyUnit.PlayAttackAnim();
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage); // �÷��̾�� �����ϰ� �׾����� Ȯ��

        yield return new WaitForSeconds(2f); // ���

        Debug.Log("����� ���� �������ϴ�");

        if(isDead) // �׾��ٸ� 
        {
            curBattleState = BattleState.Lose; // �÷��̾��� �й�
        }
        else
        {
            curBattleState = BattleState.MyTurn; // �÷��̾� ������ ��ü
        }
    }

    private void EndBattle() // ��Ʋ ���� �� ��Ʋ ����
    {
        if(curBattleState == BattleState.Won) // �÷��̾ �̰�ٸ�
        {

        }
        else if(curBattleState == BattleState.Lose) // �÷��̾ ���ٸ�
        {

        }
    }

    private void PlayerTurn() // �÷��̾� �� �ؽ�Ʈ ����
    {

    }


    public void OnAttackButton()
    {
        if (curBattleState != BattleState.MyTurn)
            return;

        StartCoroutine(PlayerAttack());
    }
}
