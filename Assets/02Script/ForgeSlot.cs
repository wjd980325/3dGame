using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ForgeSlot : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Image focus;

    private bool isFocus;
    public bool IsFocus
    {
        set
        {
            isFocus = value;
            focus.enabled = value;
        }
    }

    private InventoryItemData data;
    private Button selectBtn;

    public delegate void SelectData(InventoryItemData selectData);
    public event SelectData OnSelectData;

    public void CreateSlot()
    {
        if(TryGetComponent<Button>(out selectBtn))
        {
            selectBtn.onClick.AddListener(OnClick_SelectBtn);
            gameObject.SetActive(false);    // ��Ȱ��ȭ
        }
    }

    public void OnClick_SelectBtn()
    {
        if(!isFocus)
        {
            OnSelectData(data);
            IsFocus = true;
        }
    }

    public void RefreshSlot(InventoryItemData item)
    {
        gameObject.SetActive(true);
        data = item;
        if(GameManager.Inst.GetItemData(data.itemID, out ItemData_Entity tableData))    // ���̺� ������ Ȯ��
        {
            icon.sprite = Resources.Load<Sprite>(tableData.iconImg);
            icon.enabled = true;
            IsFocus = false;
        }
    }

    // ���� ��Ȱ��ȭ
    public void ClearSlot()
    {
        gameObject.SetActive(false);
    }
}
