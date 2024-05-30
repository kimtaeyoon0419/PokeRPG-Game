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
        public static BattleManager instance; // 싱글톤

        [Header("Monsters")]
        public GameObject myMonster; // 내 몬스터
        public GameObject enemyMonster; // 상대 몬스터
        public Transform playerBattleStation; // 플레이어 몬스터 스폰위치
        public Transform enemyBattleStation; // 상대방 몬스터 스폰위치

        public UnitProfile playerUnit { get; private set; }// 플레이어 몬스터의 UnitProfile 코드
        public UnitProfile enemyUnit { get; private set; }// 상대방 몬스터의 UnitProfile 코드

        public BattleState curBattleState { get; private set; } // 현재 배틀 턴

        [Header("UI")]
        public Text text; // 배틀 상황을 알려줄 텍스트
        public GameObject skillButton1; // 스킬 버튼
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
            curBattleState = BattleState.SelectTurn; // 현재 배틀턴을 행동 선택턴으로 설정
            StartCoroutine(SetupBattle()); // 턴 기본설정 
            skillButton1.SetActive(false); // 스킬 버튼 비활성화
        }

        IEnumerator SetupBattle()
        {
            GameObject playerGo = Instantiate(myMonster, playerBattleStation.position, Quaternion.identity); // 플레이어 몬스터 스폰
            playerGo.transform.rotation = Quaternion.Euler(0, 90, 0); // 서로 바라보기
            playerUnit = playerGo.GetComponent<UnitProfile>(); // 플레이어 몬스터의 값 가져오기
            GameObject enemyGo = Instantiate(enemyMonster, enemyBattleStation.position, Quaternion.identity); // 상대방 몬스터 스폰
            enemyGo.transform.rotation = Quaternion.Euler(0, -90, 0); // 서로 바라보기
            enemyUnit = enemyGo.GetComponent<UnitProfile>(); // 상대방 몬스터의 값 가져오기

            playerMonsterStatBox = playerMonsterStatBoxObject.GetComponent<MonsterStatBox>(); // 플레이어 몬스터 스텟박스 가져오기

            text.text = "야생의 " + enemyUnit.unitName + "이(가) 나타났다!";

            yield return new WaitForSeconds(2f); // 대기

            curBattleState = BattleState.MyTurn; // 플레이어 턴으로 교체
            PlayerTurn(); // 플레이어 턴 실행
        }

        IEnumerator PlayerAttack() // 플레이어의 공격
        {
            text.text = playerUnit.unitName + "의 공격!";
            playerUnit.PlayAttackAnim(); // 플레이어 몬스터의 공격 애니메이션 실행
            playerUnit.EnemyPosSkillAttack(enemyBattleStation); // 스킬 이펙트 소환
            bool isDead = enemyUnit.TakeDamage(playerUnit.damage); // 상대 몬스터에게 공격하고 죽었는지 확인
            curBattleState = BattleState.EnemyTurn; // 상대방 턴으로 교체
            yield return new WaitForSeconds(0.5f); // 대기
            text.text = "공격이 적중했다!";

            yield return new WaitForSeconds(2f); // 대기

            Debug.Log("플레이어의 턴이 끝났습니다");

            if (isDead) // 만약 상대 몬스터가 죽었다면
            {
                curBattleState = BattleState.Won; // 플레이어 승리 
                EndBattle(); // 배틀 종료
            }
            else // 안죽었다면
            {
                StartCoroutine(EnemyTurn()); // 상대방 턴 실행
            }
        }

        IEnumerator EnemyTurn() // 상대방의 공격
        {
            yield return new WaitForSeconds(1f); // 대기

            text.text = enemyUnit.unitName + "의 공격!";
            enemyUnit.PlayAttackAnim(); // 상대방 몬스터의 공격 애니메이션 실행
            enemyUnit.EnemyPosSkillAttack(playerBattleStation); // 상대방 몬스터의 스킬 이펙트 소환
            bool isDead = playerUnit.TakeDamage(enemyUnit.damage); // 플레이어에게 공격하고 죽었는지 확인
            yield return new WaitForSeconds(0.5f); // 대기
            text.text = "공격이 적중했다!";
            //playerMonsterStatBox.SetStatBox();

            yield return new WaitForSeconds(2f); // 대기

            Debug.Log("상대의 턴이 끝났습니다");

            if (isDead == true) // 만약 플레이어 몬스터가 죽었다면 
            {
                text.text = playerUnit.unitName + "이(가) 쓰러졌다...";
                yield return new WaitForSeconds(1.5f); // 대기
                curBattleState = BattleState.Lose; // 플레이어의 패배
                EndBattle(); // 배틀 종료
            }
            else
            {
                PlayerTurn(); // 플레이어 행동 선택창
                curBattleState = BattleState.MyTurn; // 플레이어 턴으로 교체
            }
        }

        private void EndBattle() // 배틀 종료 및 배틀 정산
        {
            if (curBattleState == BattleState.Won) // 플레이어가 이겼다면
            {
                text.text = "야생의 " + enemyUnit.unitName + "을 무찔렀다!";
            }
            else if (curBattleState == BattleState.Lose) // 플레이어가 졌다면
            {
                text.text = "김민결의 눈앞이 캄캄해졌다";
            }
        }

        private void PlayerTurn() // 플레이어 턴 텍스트 설정
        {
            text.text = "행동을 선택하세요!";
            skillButton1.SetActive(true); // 스킬 버튼 활성화
        }


        public void OnAttackButton() // 버튼 클릭했을 때 실행할 함수
        {
            if (curBattleState != BattleState.MyTurn) // 플레이어 턴이 아니라면 그냥 리턴
                return;

            skillButton1.SetActive (false); // 스킬 버튼 비활성화
            StartCoroutine(PlayerAttack()); // 플레이어 몬스터의 공격 코루틴 실행
        }
    }
}
