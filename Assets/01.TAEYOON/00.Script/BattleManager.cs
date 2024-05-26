// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance; // 싱글톤

    [SerializeField] GameObject myMonster; // 내 몬스터
    [SerializeField] GameObject enemyMonster; // 상대 몬스터

    [SerializeField] BattleState curBattleState; // 현재 배틀 턴
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
