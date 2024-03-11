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
    private int animParam_Walk = Animator.StringToHash("isWalk");   // 파라메타 아이디를 string -> int 바꿈
    private int animParam_Attack01 = Animator.StringToHash("doAttack01");

    private float moveSpeed = 12f;
    private Vector3 move = Vector3.zero;
    private bool isAttack = false;
    private GameObject obj;

    private void Awake()
    {
        // 캐릭터에 대한 테이블
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
                Debug.Log("MyCharController.cs - Awake() - joystick 참조 실패");
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
        // getaxis 서서히 멈춤, getaxisRaw 갑자기 멈춤
        move.x = Input.GetAxisRaw("Horizontal");
        move.z = Input.GetAxisRaw("Vertical");
        // UI 조이스틱 값을 함께 사용
        move.x += joystick.Horizontal;
        move.z += joystick.Vertical;
        move = move.normalized;

        isAttack = Input.GetKeyDown(KeyCode.Space);
    }

    private void Locomotion()   // 이동이나 회전 등을 처리
    {
        transform.LookAt(transform.position + move);
    }

    private void CharAnims()    // 캐릭터 애니메이션을 처리하는 함수
    {
        //anims.SetBool("isWalk", move != Vector3.zero);  // 문자열 비교라 부하가 걸림 비효율적
        anims.SetBool(animParam_Walk, move != Vector3.zero);

        // 조이스틱 혹은 스킬공격버튼들 ui적인 트리거로 변경
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
