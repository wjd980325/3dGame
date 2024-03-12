using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class DropItem : MonoBehaviour
{
    private SphereCollider col;
    private Rigidbody rig;
    private bool isDrop;    // ����� �ǰ� �ִ������� ��� �Ϸ�����
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
        col.isTrigger = true;   // �հ� ����������

        rotTrans = transform.GetChild(0);
        valueA = 0f;

        isDrop = false;

    }

    private void Update()
    {
        if(isDrop)
        {
            rotTrans.Rotate(Vector3.up * 90.0f * Time.deltaTime);   // y�� ���� ȸ��
            pos = rotTrans.position;
            valueA += Time.deltaTime;
            pos.y = dropPosY + 0.3f * Mathf.Sin(valueA);    // ��� ��ġ�� �������� 0.3 ~ -0.3 �ݺ�
            rotTrans.position = pos;
        }
    }

    // �� �ݶ��̴��� ��������
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground"))  // �ٴڰ� �浹������
        {
            rig.useGravity = false;
            rig.velocity = Vector3.zero;
            dropPosY = transform.position.y;
            isDrop = true;
        }

        if(isDrop && other.CompareTag("Player"))
        {
            Debug.Log("������ ����");
            Destroy(gameObject);
        }
    }
}
