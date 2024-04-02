using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ButtonType
{
    BT_AttackBtn,
    BT_Skill01Btn,
    BT_Skill02Btn,
    BT_Skill03Btn,
    BT_InventoryBtn,
    BT_ReturnBtn,       // 마을 귀환
}

public class UIManager : MonoBehaviour, IObserverUI
{
    private GameObject obj;
    private Image hpFill;
    private Image mpFill;
    private GameObject inventoryObj;
    private InventoryUI inventoryUI;
    private bool inventoryIsOpen;

    private Button button;

    private void Start()
    {
        InitUIManager();    // 테스트
    }

    // 해당 씬이 시작될때 초기화
    public void InitUIManager()
    {
        obj = GameObject.Find("FillHP");
        if (obj != null)
        {
            if (!obj.TryGetComponent<Image>(out hpFill))
                Debug.Log("UIManager.cs - InitUIManager() - hpFill 참조 실패");
        }
        else
            Debug.Log("UIManager.cs - InitUIManager() - FillHP 참조 실패");

        obj = GameObject.Find("FillMP");
        if (obj != null)
        {
            if (!obj.TryGetComponent<Image>(out mpFill))
                Debug.Log("UIManager.cs - InitUIManager() - mpFill 참조 실패");
        }
        else
            Debug.Log("UIManager.cs - InitUIManager() - FillMP 참조 실패");

        // 옵저버로 주체에게 등록
        obj = GameObject.Find("MainChar");
        if(obj != null)
        {
            if(obj.TryGetComponent<ISubjectUI>(out ISubjectUI subUI))
            {
                subUI.Attach(this);
            }
        }

        if(inventoryObj == null)
        {
            inventoryObj = GameObject.Find("Inventory");
            if (inventoryObj && !inventoryObj.TryGetComponent<InventoryUI>(out inventoryUI))
                Debug.Log("UIManager.cs - InitUIManager() - inventoryUI 참조 실패");

            inventoryObj.LeanScale(Vector3.zero, 0.1f);
            inventoryIsOpen = false;
        }

        obj = GameObject.Find("AttackBtn");
        if(obj != null)
        {
            if(obj.TryGetComponent<Button>(out button))
            {
                // 람다식
                button.onClick.AddListener(() => HandleButtonCilck(ButtonType.BT_AttackBtn));
            }
        }

        obj = GameObject.Find("Skill01Btn");
        if (obj != null)
        {
            if (obj.TryGetComponent<Button>(out button))
            {
                // 람다식
                button.onClick.AddListener(() => HandleButtonCilck(ButtonType.BT_Skill01Btn));
            }
        }

        obj = GameObject.Find("InventoryBtn");
        if (obj != null)
        {
            if (obj.TryGetComponent<Button>(out button))
            {
                // 람다식
                button.onClick.AddListener(() => HandleButtonCilck(ButtonType.BT_InventoryBtn));
            }
        }
    }

    private void HandleButtonCilck(ButtonType type)
    {
        switch(type)
        {
            case ButtonType.BT_AttackBtn:
                GameManager.Inst.AttackBtnEvent();
                break;
            case ButtonType.BT_Skill01Btn:
                Debug.Log("스킬1");
                break;
            case ButtonType.BT_Skill02Btn:
                break;
            case ButtonType.BT_Skill03Btn:
                break;
            case ButtonType.BT_InventoryBtn:
                ShowInventory();
                break;
            case ButtonType.BT_ReturnBtn:
                break;
        }
    }    

    public void UpdateUI(MyCharState charState)
    {
        if (hpFill != null)
            hpFill.fillAmount = charState.CurrentHP / charState.MaxHP;

        if (mpFill != null)
            mpFill.fillAmount = charState.CurrentMP / charState.MaxMP;
    }

    public void ShowInventory()
    {
        inventoryIsOpen = !inventoryIsOpen;
        if (inventoryIsOpen)
        {
            inventoryUI.RefreshInventoryUI();   // 슬롯 갱신
            inventoryObj.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInOutElastic);
        }
        else
            inventoryObj.LeanScale(Vector3.zero, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }
}
