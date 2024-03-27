using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// �������̽� : ���� ���� �Լ��θ� �̷���� �߻� Ŭ����(�Ϲ� �޼ҵ� + ���� �����Լ� ���� �ֵ��� ��Ʋ�)
public interface IDamage
{
    public void TakeDamage(int damage);     // ���� ���� �Լ�
}

// ���� �ִϸ��̼�
// ���� �������ͽ�
// ���� �ǰ� ���
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
        if (agent.velocity.sqrMagnitude > 0.1f)      // �̵� ���� üũ
            anims.SetBool(animHash_Run, true);
        else
            anims.SetBool(animHash_Run, false);
    }

    public void AttackTarget()
    {
        anims.SetTrigger(animHash_Attack);
    }
}
