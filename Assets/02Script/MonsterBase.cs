using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 몬스터 애니매이션
// 몬스터 스테이터스
// 몬스터 피격 기능
public class MonsterBase : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anims;

    private MonsterData_Entity state;

    public int CalculateDamage(int takeDamage)
    {
        int resultDamage = takeDamage;

        resultDamage += Random.Range(-10, 10);
        return resultDamage;
    }

    private void Awake()
    {
        TryGetComponent<NavMeshAgent>(out agent);
        TryGetComponent<Animator>(out anims);
    }

    private void Update()
    {
        LocomotionAnims();
    }

    private void LocomotionAnims()
    {
        if (agent.velocity.sqrMagnitude > 0.1f)      // 이동 상태 체크
            anims.SetBool("Run Forward", true);
        else
            anims.SetBool("Run Forward", false);
    }
}
