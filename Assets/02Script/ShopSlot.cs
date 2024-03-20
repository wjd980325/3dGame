using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour
{
    // �ڽ��� ������ ��ũ��Ʈ
    private Popup_Shop popupShop;

    [SerializeField]
    private Image icon;
    [SerializeField]
    private TextMeshProUGUI itemName;
    [SerializeField]
    private TextMeshProUGUI itemPrice;
    [SerializeField]
    private TextMeshProUGUI tradeCount;     // �ŷ� ����

    [SerializeField]
    private Button left;
    [SerializeField]
    private Button right;
    [SerializeField]
    private Button max;

    private InventoryItemData data;     // � ������? �? �ŷ�����

    // ���°�� �ִ� �ε�������
    private int slotIndex;
    public int SlotIndex => slotIndex;

    private int totalGold;
    public int TotalGold
    {
        get => totalGold;
        set
        {
            totalGold = value;
            if(OnTotalChange != null)
            {
                OnTotalChange();
                //OnTotalChange.Invoke();
            }
        }
    }

    public delegate void TotalGoldChange();     // �븮��
    public event TotalGoldChange OnTotalChange; // �̺�Ʈ

    private int tradeGold;      // 1�� �ŷ� �ݾ�(�ܰ�)
    private int sellMaxCount;   // �ŷ� ������ �ִ� ����
    private int curCount;       // �ŷ��Ϸ��� ����� ����
    private int itemID;

    // ���� �����Ҷ� ������ ������ٶ� �θ�, ������ �ε��� ����
    public void CreateSlot(Popup_Shop _shop, int index)
    {
        popupShop = _shop;
        slotIndex = index;
        icon.enabled = false;
        gameObject.SetActive(false);
    }

    // ���� ���� ������ ���� ����
    public void RefreshSlot(InventoryItemData newData)
    {
        gameObject.SetActive(true);
        itemID = newData.itemID;
        sellMaxCount = newData.amount;
        curCount = 0;

        // ���̺� �����Ͱ� ��ȿ �ϴٸ�
        if(GameManager.Inst.GetItemData(itemID, out ItemData_Entity tableItemData))
        {
            // �����ε��� ��ǻ�Ϳ��� �δ��� ���� �ش�
            // �ѹ� �ҷ��� �̹����� ����(RAM[HeapMemory)�� �س��ٰ� ����Ѵ�
            icon.sprite = Resources.Load<Sprite>(tableItemData.iconImg);    // �����ε�

            itemPrice.text = tableItemData.sellGold.ToString();
            tradeGold = tableItemData.sellGold;
            tradeCount.text = "0";
            icon.enabled = true;
        }
    }

    // ���� ��Ȱ��ȭ
    public void ClearSlot()
    {
        gameObject.SetActive(false);
    }

    // �����ϴ� �������� ID ���� �ѱݾ� �˷��ֱ� ���� �޼ҵ�
    public bool GetBuyCount(out int _buyItemID, out int _buyItemCount, out int _buyGold)
    {                     //out = ������ ���� �ּҸ� ������
        _buyItemID = itemID;
        _buyItemCount = curCount;
        _buyGold = totalGold;
        
        //curCount = 0;
        //TotalGold = 0;
        //tradeCount.text = curCount.ToString();
        //TotalGold = curCount * tradeGold;

        // �θ��� ItemShop Popup�� �ݾ��� �˷������

        return true;
    }

    // ApplyBtn�� �������� �Ǹ��ϴ� ���� ����
    public bool GetSellCount(out int _sellItemID, out int _sellItemCount, out int _sellGold)
    {
        _sellItemID = itemID;
        _sellItemCount = curCount;
        _sellGold = totalGold;

        //sellMaxCount -= curCount;
        //curCount = 0;
        //tradeCount.text = curCount.ToString();
        //TotalGold = curCount * tradeGold;

        // �θ��� ItemShop Popup�� �ݾ��� �˷������

        return true;
    }

    public void OnClickLeftBtn()
    {
        if (curCount > 0)
            curCount--;

        tradeCount.text = curCount.ToString();
        TotalGold = curCount * tradeGold;

        // �θ��� ItemShop Popup�� �ݾ��� �˷������
    }
    public void OnClickRightBtn()
    {
        if (sellMaxCount > curCount)
            curCount++;

        tradeCount.text = curCount.ToString();
        TotalGold = curCount * tradeGold;

        // �θ��� ItemShop Popup�� �ݾ��� �˷������
    }
    public void OnClickMaxBtn()
    {   // ����� 50 => �ŷ����� ������ 50
        curCount = sellMaxCount;
        tradeCount.text = curCount.ToString();
        TotalGold = curCount * tradeGold;

        // �θ��� ItemShop Popup�� �ݾ��� �˷������
    }

    private void Awake()
    {
        right.onClick.AddListener(OnClickRightBtn);
        left.onClick.AddListener(OnClickLeftBtn);
        max.onClick.AddListener(OnClickMaxBtn);
    }
}
