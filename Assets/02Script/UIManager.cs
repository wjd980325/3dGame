using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour, IObserverUI
{
    private GameObject obj;
    private Image hpFill;
    private Image mpFill;
    private GameObject inventoryObj;
    private InventoryUI inventoryUI;
    private bool inventoryIsOpen;

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
