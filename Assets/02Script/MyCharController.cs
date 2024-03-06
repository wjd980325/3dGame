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
    private int animParam_Walk = Animator.StringToHash("isWalk");   // 파라메타 아이디를 string -> int 바꿈
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
        // getaxis 서서히 멈춤, getaxisRaw 갑자기 멈춤
        move.x = Input.GetAxisRaw("Horizontal");
        move.z = Input.GetAxisRaw("Vertical");
        move = move.normalized;

        transform.LookAt(transform.position + move);

        //anims.SetBool("isWalk", move != Vector3.zero);  // 문자열 비교라 부하가 걸림 비효율적
        anims.SetBool(animParam_Walk, move != Vector3.zero);

        // 조이스틱 혹은 스킬공격버튼들 ui적인 트리거로 변경
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
