using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Redcode.Pools;

// �������̽� : ���� ���� �Լ��θ� �̷���� �߻� Ŭ����(�Ϲ� �޼ҵ� + ���� �����Լ� ���� �ֵ��� ��Ʋ�)
public interface IDamage
{
    public void TakeDamage(int damage);     // ���� ���� �Լ�
}

// ���� �ִϸ��̼�
// ���� �������ͽ�
// ���� �ǰ� ���
public class MonsterBase : MonoBehaviour, IDamage, IPoolObject
{
    private NavMeshAgent agent;
    private Animator anims;
    // anim hash table
    private int animHash_Run = Animator.StringToHash("Run Forward");
    private int animHash_Die = Animator.StringToHash("Die");
    private int animHash_Attack = Animator.StringToHash("Attack 01");
    private int animHash_Hit = Animator.StringToHash("Take Damage");

    private SpawnManager ownerSpawner;  // �ڽ��� ���� ���� owner  ��������� ������Ʈ Ǯ�� ȸ��ó��
    private MonsterAI monsterAI;
    private MonsterData_Entity state;
    private int currentHP;
    private Material material;

    virtual public void InitMonster(int tableID, SpawnManager newSpawn)
    {
        GameManager.Inst.GetMonsterData(tableID, out state);
        currentHP = state.maxHP;
        agent.speed = state.moveSpeed;
        agent.stoppingDistance = 10.0f;
        ownerSpawner = newSpawn;

        material.color = Color.white;

        if(TryGetComponent<MonsterAI>(out monsterAI))
        {
            monsterAI.StartAI();
        }
    }

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

        if(transform.GetChild(0).TryGetComponent<SkinnedMeshRenderer>(out SkinnedMeshRenderer render))
        {
            material = render.material;
            
        }
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

    public void TakeDamage(int damage)
    {
        if(currentHP > 0)
        {
            currentHP -= CalculateDamage(damage);
            if(currentHP <= 0)
            {
                // ���ó��
                StartCoroutine(OnDie());
            }
            else
            {// �ǰݸ� �� ��Ȳ
                StartCoroutine(OnHit());
            }
        }
    }

    IEnumerator OnDie()
    {
        anims.SetTrigger(animHash_Die);
        monsterAI.ChangeAIState(AI_State.Die);
        material.color = Color.gray;
        gameObject.layer = LayerMask.NameToLayer("Die");


        yield return YieldInstructionCache.WaitForSeconds(2f);
        // Ǯ�� ��ȯ
        ownerSpawner.ReturnPool(this);
    }

    IEnumerator OnHit()
    {
        anims.SetTrigger(animHash_Hit);

        for(int i = 0; i < 3; i++)
        {
            material.color = Color.red;
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            material.color = Color.white;
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
        }
    }

    private Projectile projectile;
    [SerializeField]
    private Transform attackTrans;
    private PoolManager poolManager;
    public PoolManager PoolMGR
    {
        get
        {
            if(poolManager == null)
            {
                TryGetComponent<PoolManager>(out poolManager);
            }
            return poolManager;
        }
    }

    public void AnimEvent_SpawnProjectile()
    {
        projectile = PoolMGR.GetFromPool<Projectile>(0);
        projectile.transform.position = attackTrans.position;
        projectile.transform.LookAt(attackTrans.position + transform.forward);

        projectile.InitProjectile(transform.forward,
                                    10f,
                                    10f,
                                    state.attackDamage,
                                    transform.tag,
                                    PoolMGR);
    }

    [SerializeField]
    private string poolName;
    public string PoolName => poolName;

    // Ǯ�� ������Ʈ�� �����Ǿ ��������� �ɶ�
    public void OnCreatedInPool()
    {
        
    }
    // �����޶�� ��û
    public void OnGettingFromPool()
    {
        
    }
}
