using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [SerializeField]
    private Transform logoTrans;

    private void Awake()
    {
        LeanTween.moveLocalY(logoTrans.gameObject, 0f, 3f).setEase(LeanTweenType.easeOutBounce);
        LeanTween.moveLocalX(logoTrans.gameObject, 0f, 3f).setEase(LeanTweenType.easeInSine);
        LeanTween.rotate(logoTrans.gameObject, Vector3.zero, 3f);
        Invoke("AutoNextScene", 4f);
    }

    private void AutoNextScene()
    {
        // 로딩 처리
        GameManager.Inst.AsyncLoadNextScene(SceneName.TitleScene);
    }
}
