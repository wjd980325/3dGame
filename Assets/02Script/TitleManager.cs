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
        if(null != newNickName && newNickName.Length >= 2)    // �弳 ���͸�, ���Ŀ� ���� ���͸�
        {
            LeanTween.scale(nickNamePopup, Vector3.zero, 0.7f).setEase(LeanTweenType.easeOutElastic);
            GameManager.Inst.CreateUserData(newNickName);
            GameManager.Inst.SaveData();    // ��������
            InitTitleScene();
            welcomeText.enabled = true;
        }
        else
        {
            // ��� �߻�
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
