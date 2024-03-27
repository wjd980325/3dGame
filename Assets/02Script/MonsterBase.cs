using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 인터페이스 : 순수 가상 함수로만 이루어진 추상 클래스(일반 메소드 + 순수 가상함수 섞인 애들을 통틀어서)
public interface IDamage
{
    public void TakeDamage(int damage);     // 순수 가상 함수
}

// 몬스터 애니매이션
// 몬스터 스테이터스
// 몬스터 피격 기능
public class MonsterBase : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anims;
    // anim hash table
    private int animHash_Run = Animator.StringToHash("Run Forward");
    private int animHash_Die = Animator.StringToHash("Die");
    private int animHash_Attack = Animator.StringToHash("Attack 01");

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
            anims.SetBool(animHash_Run, true);
        else
            anims.SetBool(animHash_Run, false);
    }

    public void AttackTarget()
    {
        anims.SetTrigger(animHash_Attack);
    }
}
