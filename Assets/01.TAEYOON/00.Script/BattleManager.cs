// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance; // �̱���

    [SerializeField] GameObject myMonster; // �� ����
    [SerializeField] GameObject enemyMonster; // ��� ����

    [SerializeField] BattleState curBattleState; // ���� ��Ʋ ��
    #region Unity_Function
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
        
    }
    #endregion
    private void CircuitTurn()
    {
       switch(curBattleState)
        {
            case BattleState.SelectTurn:
                break;
        }
    }
}
