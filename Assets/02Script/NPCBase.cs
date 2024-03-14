using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class NPCBase : MonoBehaviour
{
    private SphereCollider col;
    private Rigidbody rig;

    [SerializeField]
    private GameObject popupObj;    // ���� ������� �˾�â�� ������ �ݾҴ�

    private bool isOn = false;

    private void Awake()
    {
        if(TryGetComponent<SphereCollider>(out col))
        {
            col.isTrigger = true;
            col.radius = 2.5f;
        }

        if(TryGetComponent<Rigidbody>(out rig))
        {
            rig.useGravity = false;
            rig.isKinematic = true;     // �ٸ� ��ü�� ���ؼ� �з����� ���� �߻����� �ʴ´�
                                        // �ٸ� ��ü�� �о ���� �ִ�
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isOn && other.CompareTag("Player"))     // �˾�â ���� ���¿��� �÷��̾�� ��ħ
        {
            isOn = true;
            // �˾� ���� �������̽��� ���� ����, ����
            if (popupObj.TryGetComponent<IPopupBase>(out IPopupBase popup))
                popup.PopupOpen();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(isOn && other.CompareTag("Player"))
        {
            isOn = false;
            // �˾� �ݱ�
            if (popupObj.TryGetComponent<IPopupBase>(out IPopupBase popup))
                popup.PopupClose();
        }
    }
}
