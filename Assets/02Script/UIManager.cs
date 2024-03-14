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
        InitUIManager();    // �׽�Ʈ
    }

    // �ش� ���� ���۵ɶ� �ʱ�ȭ
    public void InitUIManager()
    {
        obj = GameObject.Find("FillHP");
        if (obj != null)
        {
            if (!obj.TryGetComponent<Image>(out hpFill))
                Debug.Log("UIManager.cs - InitUIManager() - hpFill ���� ����");
        }
        else
            Debug.Log("UIManager.cs - InitUIManager() - FillHP ���� ����");

        obj = GameObject.Find("FillMP");
        if (obj != null)
        {
            if (!obj.TryGetComponent<Image>(out mpFill))
                Debug.Log("UIManager.cs - InitUIManager() - mpFill ���� ����");
        }
        else
            Debug.Log("UIManager.cs - InitUIManager() - FillMP ���� ����");

        // �������� ��ü���� ���
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
                Debug.Log("UIManager.cs - InitUIManager() - inventoryUI ���� ����");

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
            inventoryUI.RefreshInventoryUI();   // ���� ����
            inventoryObj.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInOutElastic);
        }
        else
            inventoryObj.LeanScale(Vector3.zero, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }
}
