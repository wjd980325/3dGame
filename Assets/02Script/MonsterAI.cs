using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;   // 네이게이션 시스템 사용

public enum AI_State
{
    Idle,           // 제자리 대기
    Roaming,         // 주변 배회
    ReturnHome,     // 생성 위치로 복귀
    Chase,          // 플레이어 추적
    Attack,         // 플레이어 공격
    Die,            // 죽어있는 상태
}

// 상태, 트랜지션(변경조건), 이벤트
public class MonsterAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 spawnedPos;
    private Vector3 moveTargetPos;
    private GameObject mainTarget;

    // 현 활성화 된 상태값이 무엇인지
    private AI_State currentState;

    // AI 작동하기 위한 준비가 되었는지
    private bool isInit = false;

    private MonsterBase monster;

    private void Awake()
    {
        TryGetComponent<NavMeshAgent>(out agent);
        TryGetComponent<MonsterBase>(out monster);
        agent.SetDestination(Vector3.zero);

        StartAI();
    }

    private void SetMoveTarget(Vector3 newTargetPos)
    {
        agent.SetDestination(newTargetPos);
    }

    // 제자리 대기
    IEnumerator Idle()
    {
        yield return null;
    }
    // 스폰된 좌표를 기준으로 좌우 반경을 배회
    IEnumerator Roaming()
    {
        while(true)
        {
            yield return YieldInstructionCache.WaitForSeconds(Random.Range(4f, 6f));    // 일정 시간 대기
            moveTargetPos.x = Random.Range(-5f, 5f);
            moveTargetPos.y = 0f;
            moveTargetPos.z = Random.Range(-5f, 5f);
            SetMoveTarget(spawnedPos + moveTargetPos);
        }
    }
    // 스폰 위치로 복귀하는 상태
    IEnumerator ReturnHome()
    {
        SetMoveTarget(spawnedPos);
        while(true)
        {
            yield return YieldInstructionCache.WaitForSeconds(1f);
            if(agent.remainingDistance < 2f)
            {
                ChangeAIState(AI_State.Roaming);     // 로밍 상태 전이
            }
        }
    }
    // 몬스터가 플레이어를 공격하기 위해서 사거리 내로 추적하고 있는 상태
    IEnumerator Chase()
    {
        while(mainTarget != null)
        {
            if(GetDistanceToTarget() < 2.5f)    // 사거리 체크
            {
                ChangeAIState(AI_State.Attack); // 공격 상태
            }
            else        // 이동 목표 좌표를 타겟의 위치로 갱신
            {
                SetMoveTarget(mainTarget.transform.position);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.5f);
        }
        ChangeAIState(AI_State.ReturnHome);      // 리턴 홈 // 타겟이 사망
    }
    // 공격 속도 의거하여 플레이어 공격
    IEnumerator Attack()
    {
        while(mainTarget != null)
        {
            if(GetDistanceToTarget() > 2.5f) //사거리 체크
            {
                ChangeAIState(AI_State.Chase);   // 추적
            }

            transform.LookAt(mainTarget.transform);     // 상대방을 쳐다보게
            monster.AttackTarget();      // 애니메이션 출력

            yield return YieldInstructionCache.WaitForSeconds(1f);// 공격속도
        }
    }
    IEnumerator Die()
    {
        while(true)
        {
            agent.isStopped = true;
            yield return YieldInstructionCache.WaitForSeconds(1f);
        }
    }

    // 타겟과의 거리를 계산한 함수
    private float GetDistanceToTarget()
    {
        if(mainTarget != null)
        {
            return Vector3.Distance(transform.position, mainTarget.transform.position);     // 성능이 좋지 않아서
            //(transform.position - mainTarget.transform.position).sqrMagnitude;    // 성능 우수
        }
        return -1f;     // 에러 체크
    }

    // 새로운 공격 상대방을 지정하는 메소드
    public void SetTarget(GameObject newTarget)
    {
        if(currentState == AI_State.Roaming || currentState == AI_State.Idle)
        {
            mainTarget = newTarget;
            ChangeAIState(AI_State.Chase);
        }
    }

    // AI 상태값을 전이(변경) 해주는 메소드
    public void ChangeAIState(AI_State newState)
    {
        if(isInit)  // AI가 작동하고 있을때만 상태 전이 가능
        {
            StopCoroutine(currentState.ToString());     // 수행하고 있는 상태 멈추고
            currentState = newState;        // 상태값 변경
            StartCoroutine(currentState.ToString());    // 신규 상태 동작
        }
    }

    // AI 시작 시키는 메소드
    public void StartAI()
    {
        isInit = true;
        currentState = AI_State.Roaming;
        ChangeAIState(AI_State.Roaming);
        mainTarget = null;
        agent.isStopped = false;
        spawnedPos = transform.position;
    }
}
