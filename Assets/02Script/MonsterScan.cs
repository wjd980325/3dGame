using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class MonsterScan : MonoBehaviour
{
    private MonsterAI monsterAI;
    private SphereCollider col;

    private void Awake()
    {
        if(!transform.root.TryGetComponent<MonsterAI>(out monsterAI))
        {
            Debug.Log("MonsterScan.cs - Awake() - monsterAI 참조 실패");
        }

        if(TryGetComponent<SphereCollider>(out col))
        {
            col.isTrigger = true;
            col.radius = 12.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && monsterAI != null)
        {
            // monsterAI 적을 인식했다
            monsterAI.SetTarget(other.gameObject);
        }
    }
}
