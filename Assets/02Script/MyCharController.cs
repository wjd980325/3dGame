using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class MyCharController : MonoBehaviour
{
    private CapsuleCollider col;
    private Rigidbody rig;
    private FixedJoystick joystick;

    private Animator anims;
    private int animParam_Walk = Animator.StringToHash("isWalk");   // �Ķ��Ÿ ���̵� string -> int �ٲ�
    private int animParam_Attack01 = Animator.StringToHash("doAttack01");

    private float moveSpeed = 12f;
    private Vector3 move = Vector3.zero;
    private bool isAttack = false;
    private GameObject obj;

    private void Awake()
    {
        // ĳ���Ϳ� ���� ���̺�
        if(TryGetComponent<CapsuleCollider>(out col))
        {
            col.height = 1.6f;
        }
        TryGetComponent<Rigidbody>(out rig);
        TryGetComponent<Animator>(out anims);
        obj = GameObject.Find("FixedJoystick");
        if(obj != null)
        {
            if (!obj.TryGetComponent<FixedJoystick>(out joystick))
                Debug.Log("MyCharController.cs - Awake() - joystick ���� ����");
        }
    }

    private void Update()
    {
        UserInput();

        Locomotion();

        CharAnims();
    }

    private void UserInput()
    {
        // getaxis ������ ����, getaxisRaw ���ڱ� ����
        move.x = Input.GetAxisRaw("Horizontal");
        move.z = Input.GetAxisRaw("Vertical");
        // UI ���̽�ƽ ���� �Բ� ���
        move.x += joystick.Horizontal;
        move.z += joystick.Vertical;
        move = move.normalized;

        isAttack = Input.GetKeyDown(KeyCode.Space);
    }

    private void Locomotion()   // �̵��̳� ȸ�� ���� ó��
    {
        transform.LookAt(transform.position + move);
    }

    private void CharAnims()    // ĳ���� �ִϸ��̼��� ó���ϴ� �Լ�
    {
        //anims.SetBool("isWalk", move != Vector3.zero);  // ���ڿ� �񱳶� ���ϰ� �ɸ� ��ȿ����
        anims.SetBool(animParam_Walk, move != Vector3.zero);

        // ���̽�ƽ Ȥ�� ��ų���ݹ�ư�� ui���� Ʈ���ŷ� ����
        if (isAttack)
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
