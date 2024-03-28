using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
    
public enum SpawnType
{
    once,       // 한꺼번에 여러마리 스폰
    repeat,     // 지속시간동안 틱을 돌려서 지속 스폰방식
}

[RequireComponent(typeof(BoxCollider))]
public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private int maxCount;   // 해당영역에 동시에 살아 있을 수 있는 몬스터의 최대 수
    private int curCount = 0;   // 해당영역에 현재 살아 있는 숫자
    [SerializeField]
    private SpawnType spawnType;    // 어떤 방식으로 스폰할건지

    [SerializeField]
    private int spawnMonsterTableID;    // 해당 영역에 스폰되어야 하는 몬스터의 아이디

    private PoolManager poolManager;    // 오브젝트 풀

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
        // 오브젝트 풀에서 꺼내오기
        monster = POOLManager.GetFromPool<MonsterBase>(0);  // 자동 활성화

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
