using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Redcode.Pools;

// 인터페이스 : 순수 가상 함수로만 이루어진 추상 클래스(일반 메소드 + 순수 가상함수 섞인 애들을 통틀어서)
public interface IDamage
{
    public void TakeDamage(int damage);     // 순수 가상 함수
}

// 몬스터 애니매이션
// 몬스터 스테이터스
// 몬스터 피격 기능
public class MonsterBase : MonoBehaviour, IDamage, IPoolObject
{
    private NavMeshAgent agent;
    private Animator anims;
    // anim hash table
    private int animHash_Run = Animator.StringToHash("Run Forward");
    private int animHash_Die = Animator.StringToHash("Die");
    private int animHash_Attack = Animator.StringToHash("Attack 01");
    private int animHash_Hit = Animator.StringToHash("Take Damage");

    private SpawnManager ownerSpawner;  // 자신을 스폰 해준 owner  사망했을때 오브젝트 풀에 회수처리
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
        if (agent.velocity.sqrMagnitude > 0.1f)      // 이동 상태 체크
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
                // 사망처리
                StartCoroutine(OnDie());
            }
            else
            {// 피격만 한 상황
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
        // 풀에 반환
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

    // 풀에 오브젝트가 생성되어서 관리대상이 될때
    public void OnCreatedInPool()
    {
        
    }
    // 꺼내달라고 요청
    public void OnGettingFromPool()
    {
        
    }
}
