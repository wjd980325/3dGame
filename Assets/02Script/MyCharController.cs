using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

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

        //isAttack = Input.GetKeyDown(KeyCode.Space);  // �׽�Ʈ
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
        {
            anims.SetTrigger(animParam_Attack01);
            isAttack = false;
        } 
    }

    float lastAttackTime = 0;
    public void TryAttack()
    {
        if(!isAttack && lastAttackTime < Time.time)
        {
            isAttack = true;
            lastAttackTime = Time.time + 2f;    // ���� ������ 2��
        }
    }

    private void FixedUpdate()
    {
        if (move != Vector3.zero)
        {
            rig.MovePosition(rig.position + move * moveSpeed * Time.fixedDeltaTime);
        }
    }

    [SerializeField]
    private Transform attackTransform;
    private PoolManager poolManager;
    public PoolManager PoolMGR
    {
        get
        {
            if(poolManager == null)
            {
                TryGetComponent<PoolManager>(out poolManager);
            }
            return poolManager;
        }
    }
    private Projectile projectile;


    public void AnimEvent_SpawnProjectile()
    {
        Debug.Log("�ִϸ��̼�");
        projectile = PoolMGR.GetFromPool<Projectile>(0);
        projectile.transform.position = attackTransform.position;
        // ���� ��ġ + ĳ���� ���� ����
        projectile.transform.LookAt(attackTransform.position + transform.forward);

        projectile.InitProjectile(transform.forward,
                                    12.0f,
                                    10.0f,
                                    50,
                                    transform.tag,
                                    PoolMGR);
    }
}
