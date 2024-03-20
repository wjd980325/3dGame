using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup_Shop : MonoBehaviour, IPopupBase
{
    [SerializeField]
    private GameObject shopSlotPrefab;  // ���� ������
    [SerializeField]
    private RectTransform sellViewContent;
    [SerializeField]
    private RectTransform buyViewContent;
    [SerializeField]
    private TextMeshProUGUI balanceText;   // ������ �����ݾ� ǥ��
    [SerializeField]
    private TextMeshProUGUI tradeText;      // �ŷ� ����� �� �հ� �ݾ� ǥ��
    [SerializeField]
    private Button sellTapBtn;
    [SerializeField]
    private Button buyTapBtn;
    [SerializeField]
    private Button tradeBtn;
    [SerializeField]
    private GameObject sellView;
    [SerializeField]
    private GameObject buyView;

    List<ShopSlot> sellSlotList = new List<ShopSlot>();
    List<ShopSlot> buySlotList = new List<ShopSlot>();
    List<InventoryItemData> dataList;

    private Inventory inventory;
    private ShopSlot shopSlot;

    private void Awake()
    {
        InitPopup();
        PopupClose();
    }

    private void InitPopup()    // �� �ǿ� ���Ե� ����
    {
        inventory = GameManager.Inst.INVEN;

        // ������ ���ο��� �Ǹ��ϴ� ����Ʈ
        for(int i = 0; i < inventory.MAXCounter; i++)
        {
            if(Instantiate(shopSlotPrefab, sellViewContent).TryGetComponent<ShopSlot>(out shopSlot))
            {
                shopSlot.CreateSlot(this, i);   // ������ �ε��� �ο�
                shopSlot.gameObject.name = $"SellSlot_{i}";
                // ��������Ʈ �̺�Ʈ ���
                shopSlot.OnTotalChange += CalculateGold;

                sellSlotList.Add(shopSlot);
            }
        }

        // ������ �������� �Ǹ��ϴ� ����Ʈ
        for(int i = 0; i < 4; i++)
        {
            if(Instantiate(shopSlotPrefab, buyViewContent).TryGetComponent<ShopSlot>(out shopSlot))
            {
                shopSlot.CreateSlot(this, i);
                shopSlot.gameObject.name = $"BuySlot_{i}";
                shopSlot.OnTotalChange += CalculateGold;

                buySlotList.Add(shopSlot);
            }
        }

        sellTapBtn.onClick.AddListener(OnClick_SellTap);
        buyTapBtn.onClick.AddListener(OnClick_BuyTap);
        tradeBtn.onClick.AddListener(OnClick_ApplyBtn);

        totalGold = 0;
        RefreshGold();
    }

    private int totalGold;

    public void RefreshGold()
    {
        // totalGold ���

        tradeText.text = totalGold.ToString();
        balanceText.text = GameManager.Inst.PlayerGold.ToString();
    }

    public void CalculateGold()
    {
        totalGold = 0;

        if(sellView.activeSelf)     // �Ǹ�â�� Ȱ��ȭ �Ǿ� ������
        {
            for(int i = 0; i < sellSlotList.Count; i++)
            {
                if(sellSlotList[i].isActiveAndEnabled)
                {
                    totalGold += sellSlotList[i].TotalGold;
                }
            }
        }
        else   // ����â�� Ȱ��ȭ �Ǿ� ������
        {
            for(int i = 0; i < buySlotList.Count; i++)
            {
                if(buySlotList[i].isActiveAndEnabled)
                {
                    totalGold += buySlotList[i].TotalGold;
                }
            }
        }
        RefreshGold();
    }

    // �κ��丮�� �ִ� ������ �����ؼ� ���� ���Կ� �����͸� ����
    private void RefreshSellViewData()
    {
        balanceText.text = GameManager.Inst.PlayerGold.ToString();
        dataList = GameManager.Inst.INVEN.GetItemList();    // �ֽ� ������ ����

        for(int i = 0; i < inventory.MAXCounter; i++)
        {
            if(i < inventory.CurItemCount && -1 < dataList[i].itemID)      // �������� �ִ� ����
            {
                sellSlotList[i].RefreshSlot(dataList[i]);
            }
            else     // �������� ���� ����
            {
                sellSlotList[i].ClearSlot();    // ���â���� ������ �ʰ� ��Ȱ��ȭ
            }
        }
        totalGold = 0;
        RefreshGold();
    }

    InventoryItemData tempData = new InventoryItemData();
    // ������ �ǸŸ���Ʈ ����
    private void RefreshBuyViewData()
    {
        for(int i = 0; i < 4; i++)
        {
            tempData.itemID = 2001001 + i;
            tempData.amount = 999;      // �������κ��� �Ѳ����� ������ �� �ִ� �ִ� ����
            buySlotList[i].RefreshSlot(tempData);
        }
        totalGold = 0;
        RefreshGold();
    }

    public void OnClick_SellTap()
    {
        RefreshSellViewData();
        sellView.SetActive(true);
        buyView.SetActive(false);
    }

    public void OnClick_BuyTap()
    {
        RefreshBuyViewData();
        buyView.SetActive(true);
        sellView.SetActive(false);
    }

    int itemID, tradeCount, tradeGold;
    public void OnClick_ApplyBtn()
    {
        if(sellView.activeSelf)     // �Ǹ��� ����
        {
            for(int i = inventory.CurItemCount - 1; i >= 0; i--)
            {
                sellSlotList[i].GetSellCount(out itemID, out tradeCount, out tradeGold);
                GameManager.Inst.PlayerGold += tradeGold;   // ��� ����

                InventoryItemData tradeData = new InventoryItemData();
                tradeData.itemID = itemID;
                tradeData.amount = tradeCount;
                inventory.DeleteItem(tradeData);    // ������ ����
            }
            OnClick_SellTap();      // �Ǹ��� ����
        }
        else     // ������ ����
        {
            totalGold = 0;

            for(int i = 0; i < 4; i++)
            {
                buySlotList[i].GetBuyCount(out itemID, out tradeCount, out tradeGold);
                totalGold += tradeGold;
            }

            if (totalGold <= GameManager.Inst.PlayerGold)    // �÷��̾� �ܾ��� �ŷ��Ϸ��� ����� �ݾ׺��� �۴ٸ�
            {
                GameManager.Inst.PlayerGold -= totalGold;   // ��� ����

                for(int i = 0; i < 4; i++)
                {
                    buySlotList[i].GetBuyCount(out itemID, out tradeCount, out tradeGold);
                    if(tradeCount > 0)
                    {
                        InventoryItemData tradeData = new InventoryItemData();
                        tradeData.itemID = itemID;
                        tradeData.amount = tradeCount;
                        inventory.AddItem(tradeData);
                    }
                }
            }
            OnClick_BuyTap();   // ����â ����
        }
    }


    public void PopupClose()
    {
        gameObject.LeanScale(Vector3.zero, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }

    public void PopupOpen()
    {
        // ���°��� ����
        OnClick_BuyTap();
        gameObject.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }
}
