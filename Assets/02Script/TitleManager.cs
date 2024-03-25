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

    // ���̺� ������ ������ Ȯ���ϰ� �׿� ���� �ؽ�Ʈ�� Ȱ��ȭ
    private void InitTitleScene()
    {
        if(GameManager.Inst.CheckData())    // ���� �����Ͱ� ������
        {
            welcomeText.text = $"{GameManager.Inst.PlayerName} �� ȯ���մϴ�.\n��ġ�� ����.";
            havePlayerInfo = true;
        }
        else
        {
            welcomeText.text = "����Ϸ��� ��ġ �ϼ���.";
            havePlayerInfo = false;
        }
    }

    public void EnterBtn()
    {
        if (havePlayerInfo)
            GameManager.Inst.AsyncLoadNextScene(SceneName.BaseTown);    // ����
        else
        {
            // ���� ���� ����
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
        if(newNickName.Length >= 2)    // �弳 ���͸�, ���Ŀ� ���� ���͸�
        {

        }
        else
        {
            // ��� �߻�
        }
    }
}
