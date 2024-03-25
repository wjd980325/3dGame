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
        if(newNickName.Length >= 2)    // 욕설 필터링, 형식에 대한 필터링
        {

        }
        else
        {
            // 경고 발생
        }
    }
}
