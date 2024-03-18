using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_Shop : MonoBehaviour, IPopupBase
{
    public void PopupClose()
    {
        gameObject.LeanScale(Vector3.zero, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }

    public void PopupOpen()
    {
        // 상태값을 갱신
        gameObject.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }
}
