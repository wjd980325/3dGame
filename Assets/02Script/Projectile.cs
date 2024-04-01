using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class Projectile : MonoBehaviour, IPoolObject
{
    [SerializeField]
    private float lifeTime = 10f;   // 새롭게 스폰한 뒤로 10초이상 날라가게 되면 자연 소멸
    private float selfDestroyTime;  // 소멸시간 체크하기 위한
    
    [SerializeField]
    private string poolManagerName;
    private PoolManager poolManager;
    private string ownerTag;    // 프로직테아리 생성한 오브젝트 태그
    private bool isInit;
    private ParticleSystem particle;
    private GameObject ham;

    private Vector3 moveDir;
    private float moveSpeed;
    private int attackDamage;

    public void InitProjectile(Vector3 newDir, float newSpeed, float newLifeTime,
                                int newDamage, string newTag, PoolManager pool)
    {
        moveDir = newDir;
        moveSpeed = newSpeed;
        lifeTime = newLifeTime;
        selfDestroyTime = Time.time + lifeTime;
        attackDamage = newDamage;
        ownerTag = newTag;
        poolManager = pool;

        if(transform.GetChild(1).TryGetComponent<ParticleSystem>(out particle))
        {

        }

        ham = transform.GetChild(0).gameObject;
        if(ham != null)
        {
            ham.SetActive(true);    // 이펙트 터지는 타이밍 해머 안보이게 처리
        }
        isInit = true;
    }

    private void Update()
    {
        if(isInit)  // 초기화가 완료된 투사체라면
        {
            transform.position += Time.deltaTime * moveSpeed * moveDir;
            transform.Rotate(Time.deltaTime * 720f * -Vector3.forward);
            if(Time.time > selfDestroyTime)
            {
                Explosion();
            }
        }
    }

    // 충돌 처리
    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(ownerTag))
        {
            Explosion();
        }
    }

    private void Explosion()
    {
        ApplyDamage();
        particle.Play();
        isInit = false;
        ham.SetActive(false);
        Invoke("ReturnPool", 3f);   // 이펙트 재생이 마무리가 되면 풀에 반환 처리
    }

    private void ApplyDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);
        for(int i = 0; i < colliders.Length; i++)
        {
            // 오너 태그가 없는 대상에게 데미지 부여
            if(!colliders[i].CompareTag(ownerTag) && colliders[i].TryGetComponent<IDamage>(out IDamage damage))
            {
                damage.TakeDamage(attackDamage);
            }
        }
    }
    // 반환 처리
    private void ReturnPool()
    {
        poolManager.TakeToPool<Projectile>(poolManagerName, this);
    }

    public void OnCreatedInPool()
    {
        
    }

    public void OnGettingFromPool()
    {

    }

      
}
