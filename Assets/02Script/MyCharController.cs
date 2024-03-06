using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class MyCharController : MonoBehaviour
{
    private CapsuleCollider col;
    private Rigidbody rig;
    private Animator anims;
    private int animParam_Walk = Animator.StringToHash("isWalk");   // �Ķ��Ÿ ���̵� string -> int �ٲ�
    private int animParam_Attack01 = Animator.StringToHash("doAttack01");

    private float moveSpeed = 4f;
    private Vector3 move = Vector3.zero;

    private void Awake()
    {

        if(TryGetComponent<CapsuleCollider>(out col))
        {
            col.height = 1.6f;
        }
        TryGetComponent<Rigidbody>(out rig);
        TryGetComponent<Animator>(out anims);
    }

    private void Update()
    {
        // getaxis ������ ����, getaxisRaw ���ڱ� ����
        move.x = Input.GetAxisRaw("Horizontal");
        move.z = Input.GetAxisRaw("Vertical");
        move = move.normalized;

        transform.LookAt(transform.position + move);

        //anims.SetBool("isWalk", move != Vector3.zero);  // ���ڿ� �񱳶� ���ϰ� �ɸ� ��ȿ����
        anims.SetBool(animParam_Walk, move != Vector3.zero);

        // ���̽�ƽ Ȥ�� ��ų���ݹ�ư�� ui���� Ʈ���ŷ� ����
        if (Input.GetKeyDown(KeyCode.Space))
            anims.SetTrigger(animParam_Attack01);

    }

    private void FixedUpdate()
    {
        if (move != Vector3.zero)
        {
            rig.MovePosition(rig.position + move * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
