using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI welcomeText;
    [SerializeField]
    private GameObject nickNamePopup;
    private bool havePlayerInfo;

    private void Awake()
    {
        InitTitleScene();
    }

    // 세이브 파일의 유무를 확인하고 그에 따른 텍스트들 활성화
    private void InitTitleScene()
    {
        if(GameManager.Inst.CheckData())    // 유저 데이터가 있으면
        {
            welcomeText.text = $"{GameManager.Inst.PlayerName} 님 환영합니다.\n터치시 시작.";
            havePlayerInfo = true;
        }
        else
        {
            welcomeText.text = "계속하려면 터치 하세요.";
            havePlayerInfo = false;
        }
    }

    public void EnterBtn()
    {
        if (havePlayerInfo)
            GameManager.Inst.AsyncLoadNextScene(SceneName.BaseTown);    // 접속
        else
        {
            // 계정 생성 로직
            LeanTween.scale(nickNamePopup, Vector3.one, 0.7f).setEase(LeanTweenType.easeOutElastic);
            welcomeText.enabled = false;
        }
    }

    public void DeleteBtn()
    {
        GameManager.Inst.DeleteData();
        InitTitleScene();
    }

    private string newNickName;
    public void InputField(string input)
    {
        newNickName = input;
    }

    public void CreateUserInfo()
    {
        if(null != newNickName && newNickName.Length >= 2)    // 욕설 필터링, 형식에 대한 필터링
        {
            LeanTween.scale(nickNamePopup, Vector3.zero, 0.7f).setEase(LeanTweenType.easeOutElastic);
            GameManager.Inst.CreateUserData(newNickName);
            GameManager.Inst.SaveData();    // 파일저장
            InitTitleScene();
            welcomeText.enabled = true;
        }
        else
        {
            // 경고 발생
            WarningTEXT();
        }
    }

    #region WarningText

    [SerializeField]
    TextMeshProUGUI waningText;

    void WarningTEXT()
    {
        Color fromColor = Color.red;
        Color toColor = Color.red;
        fromColor.a = 0f;
        toColor.a = 1f;

        LeanTween.value(waningText.gameObject, UpdateValue, fromColor, toColor, 1f).setEase(LeanTweenType.easeInQuad);
        LeanTween.value(waningText.gameObject, UpdateValue, toColor, fromColor, 1f).setDelay(1.5f).setEase(LeanTweenType.easeInQuad);

    }

    void UpdateValue(Color value)
    {
        waningText.color = value;
    }

    #endregion
}
