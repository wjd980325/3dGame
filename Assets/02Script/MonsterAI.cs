using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;   // 네이게이션 시스템 사용

public class MonsterAI : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Awake()
    {
        TryGetComponent<NavMeshAgent>(out agent);
        agent.SetDestination(Vector3.zero);
    }
}
