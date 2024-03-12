using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class DropItem : MonoBehaviour
{
    private SphereCollider col;
    private Rigidbody rig;
    private bool isDrop;    // 드랍이 되고 있는중인지 드랍 완료인지
    private Vector3 pos;
    private Transform rotTrans;
    private float dropPosY;
    private float valueA;


    private void Awake()
    {
        TryGetComponent<Rigidbody>(out rig);
        TryGetComponent<SphereCollider>(out col);

        rig.useGravity = true;
        rig.AddForce(Vector3.up * 5f, ForceMode.Impulse);

        col.radius = 0.5f;
        col.isTrigger = true;   // 뚫고 지나가도록

        rotTrans = transform.GetChild(0);
        valueA = 0f;

        isDrop = false;

    }

    private void Update()
    {
        if(isDrop)
        {
            rotTrans.Rotate(Vector3.up * 90.0f * Time.deltaTime);   // y축 기준 회전
            pos = rotTrans.position;
            valueA += Time.deltaTime;
            pos.y = dropPosY + 0.3f * Mathf.Sin(valueA);    // 드랍 위치를 기준으로 0.3 ~ -0.3 반복
            rotTrans.position = pos;
        }
    }

    // 두 콜라이더가 겹쳐질때
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground"))  // 바닥과 충돌했을때
        {
            rig.useGravity = false;
            rig.velocity = Vector3.zero;
            dropPosY = transform.position.y;
            isDrop = true;
        }

        if(isDrop && other.CompareTag("Player"))
        {
            Debug.Log("아이템 습득");
            Destroy(gameObject);
        }
    }
}
