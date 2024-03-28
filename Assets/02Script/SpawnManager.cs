using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
    
public enum SpawnType
{
    once,       // �Ѳ����� �������� ����
    repeat,     // ���ӽð����� ƽ�� ������ ���� �������
}

[RequireComponent(typeof(BoxCollider))]
public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private int maxCount;   // �ش翵���� ���ÿ� ��� ���� �� �ִ� ������ �ִ� ��
    private int curCount = 0;   // �ش翵���� ���� ��� �ִ� ����
    [SerializeField]
    private SpawnType spawnType;    // � ������� �����Ұ���

    [SerializeField]
    private int spawnMonsterTableID;    // �ش� ������ �����Ǿ�� �ϴ� ������ ���̵�

    private PoolManager poolManager;    // ������Ʈ Ǯ

    public PoolManager POOLManager
    {
        get
        {
            if(poolManager = null)
            {
                TryGetComponent<PoolManager>(out poolManager);
            }
            return poolManager;
        }
    }

    private void Awake()
    {
        if(TryGetComponent<BoxCollider>(out BoxCollider col))
        {
            col.isTrigger = true;
            col.size = new Vector3(20f, 20f, 20);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine("TrySpawn");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine("TrySpawn");
        }
    }

    IEnumerator TrySpawn()
    {
        while(true)
        {
            yield return YieldInstructionCache.WaitForSeconds(2f);
            if(curCount < maxCount)
            {
                SpawnUnit();
            }
        }
    }

    private Vector3 spawnPos;
    private MonsterBase monster;
    private void SpawnUnit()
    {
        // ������Ʈ Ǯ���� ��������
        monster = POOLManager.GetFromPool<MonsterBase>(0);  // �ڵ� Ȱ��ȭ

        if(monster != null)
        {
            spawnPos = transform.position;
            spawnPos.x += Random.Range(-10f, 10f);
            spawnPos.z += Random.Range(-10f, 10f);

            monster.transform.position = spawnPos;
            monster.InitMonster(spawnMonsterTableID, this);
            curCount++;
        }
    }

    public void ReturnPool(MonsterBase returnMonster)
    {
        poolManager.TakeToPool<MonsterBase>(returnMonster.PoolName, returnMonster);
        curCount--;
    }
}
