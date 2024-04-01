using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class Projectile : MonoBehaviour, IPoolObject
{
    [SerializeField]
    private float lifeTime = 10f;   // ���Ӱ� ������ �ڷ� 10���̻� ���󰡰� �Ǹ� �ڿ� �Ҹ�
    private float selfDestroyTime;  // �Ҹ�ð� üũ�ϱ� ����
    
    [SerializeField]
    private string poolManagerName;
    private PoolManager poolManager;
    private string ownerTag;    // �������׾Ƹ� ������ ������Ʈ �±�
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
            ham.SetActive(true);    // ����Ʈ ������ Ÿ�̹� �ظ� �Ⱥ��̰� ó��
        }
        isInit = true;
    }

    private void Update()
    {
        if(isInit)  // �ʱ�ȭ�� �Ϸ�� ����ü���
        {
            transform.position += Time.deltaTime * moveSpeed * moveDir;
            transform.Rotate(Time.deltaTime * 720f * -Vector3.forward);
            if(Time.time > selfDestroyTime)
            {
                Explosion();
            }
        }
    }

    // �浹 ó��
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
        Invoke("ReturnPool", 3f);   // ����Ʈ ����� �������� �Ǹ� Ǯ�� ��ȯ ó��
    }

    private void ApplyDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);
        for(int i = 0; i < colliders.Length; i++)
        {
            // ���� �±װ� ���� ��󿡰� ������ �ο�
            if(!colliders[i].CompareTag(ownerTag) && colliders[i].TryGetComponent<IDamage>(out IDamage damage))
            {
                damage.TakeDamage(attackDamage);
            }
        }
    }
    // ��ȯ ó��
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
