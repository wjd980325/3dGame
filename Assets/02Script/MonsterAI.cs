using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;   // ���̰��̼� �ý��� ���

public enum AI_State
{
    Idle,           // ���ڸ� ���
    Roaming,         // �ֺ� ��ȸ
    ReturnHome,     // ���� ��ġ�� ����
    Chase,          // �÷��̾� ����
    Attack,         // �÷��̾� ����
    Die,            // �׾��ִ� ����
}

// ����, Ʈ������(��������), �̺�Ʈ
public class MonsterAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 spawnedPos;
    private Vector3 moveTargetPos;
    private GameObject mainTarget;

    // �� Ȱ��ȭ �� ���°��� ��������
    private AI_State currentState;

    // AI �۵��ϱ� ���� �غ� �Ǿ�����
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

    // ���ڸ� ���
    IEnumerator Idle()
    {
        yield return null;
    }
    // ������ ��ǥ�� �������� �¿� �ݰ��� ��ȸ
    IEnumerator Roaming()
    {
        while(true)
        {
            yield return YieldInstructionCache.WaitForSeconds(Random.Range(4f, 6f));    // ���� �ð� ���
            moveTargetPos.x = Random.Range(-5f, 5f);
            moveTargetPos.y = 0f;
            moveTargetPos.z = Random.Range(-5f, 5f);
            SetMoveTarget(spawnedPos + moveTargetPos);
        }
    }
    // ���� ��ġ�� �����ϴ� ����
    IEnumerator ReturnHome()
    {
        SetMoveTarget(spawnedPos);
        while(true)
        {
            yield return YieldInstructionCache.WaitForSeconds(1f);
            if(agent.remainingDistance < 2f)
            {
                ChangeAIState(AI_State.Roaming);     // �ι� ���� ����
            }
        }
    }
    // ���Ͱ� �÷��̾ �����ϱ� ���ؼ� ��Ÿ� ���� �����ϰ� �ִ� ����
    IEnumerator Chase()
    {
        while(mainTarget != null)
        {
            if(GetDistanceToTarget() < 2.5f)    // ��Ÿ� üũ
            {
                ChangeAIState(AI_State.Attack); // ���� ����
            }
            else        // �̵� ��ǥ ��ǥ�� Ÿ���� ��ġ�� ����
            {
                SetMoveTarget(mainTarget.transform.position);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.5f);
        }
        ChangeAIState(AI_State.ReturnHome);      // ���� Ȩ // Ÿ���� ���
    }
    // ���� �ӵ� �ǰ��Ͽ� �÷��̾� ����
    IEnumerator Attack()
    {
        while(mainTarget != null)
        {
            if(GetDistanceToTarget() > 2.5f) //��Ÿ� üũ
            {
                ChangeAIState(AI_State.Chase);   // ����
            }

            transform.LookAt(mainTarget.transform);     // ������ �Ĵٺ���
            monster.AttackTarget();      // �ִϸ��̼� ���

            yield return YieldInstructionCache.WaitForSeconds(1f);// ���ݼӵ�
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

    // Ÿ�ٰ��� �Ÿ��� ����� �Լ�
    private float GetDistanceToTarget()
    {
        if(mainTarget != null)
        {
            return Vector3.Distance(transform.position, mainTarget.transform.position);     // ������ ���� �ʾƼ�
            //(transform.position - mainTarget.transform.position).sqrMagnitude;    // ���� ���
        }
        return -1f;     // ���� üũ
    }

    // ���ο� ���� ������ �����ϴ� �޼ҵ�
    public void SetTarget(GameObject newTarget)
    {
        if(currentState == AI_State.Roaming || currentState == AI_State.Idle)
        {
            mainTarget = newTarget;
            ChangeAIState(AI_State.Chase);
        }
    }

    // AI ���°��� ����(����) ���ִ� �޼ҵ�
    public void ChangeAIState(AI_State newState)
    {
        if(isInit)  // AI�� �۵��ϰ� �������� ���� ���� ����
        {
            StopCoroutine(currentState.ToString());     // �����ϰ� �ִ� ���� ���߰�
            currentState = newState;        // ���°� ����
            StartCoroutine(currentState.ToString());    // �ű� ���� ����
        }
    }

    // AI ���� ��Ű�� �޼ҵ�
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
