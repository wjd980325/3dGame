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
    private GameObject popupObj;    // 각자 담당중인 팝업창을 열었다 닫았다

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
            rig.isKinematic = true;     // 다른 강체에 의해서 밀려나는 일이 발생하지 않는다
                                        // 다른 강체를 밀어낼 수는 있다
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isOn && other.CompareTag("Player"))     // 팝업창 닫힌 상태에서 플레이어와 겹침
        {
            isOn = true;
            // 팝업 오픈 인터페이스를 통해 오픈, 닫힘
            if (popupObj.TryGetComponent<IPopupBase>(out IPopupBase popup))
                popup.PopupOpen();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(isOn && other.CompareTag("Player"))
        {
            isOn = false;
            // 팝업 닫기
            if (popupObj.TryGetComponent<IPopupBase>(out IPopupBase popup))
                popup.PopupClose();
        }
    }
}
