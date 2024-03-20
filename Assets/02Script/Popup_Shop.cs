using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup_Shop : MonoBehaviour, IPopupBase
{
    [SerializeField]
    private GameObject shopSlotPrefab;  // 슬롯 프리팹
    [SerializeField]
    private RectTransform sellViewContent;
    [SerializeField]
    private RectTransform buyViewContent;
    [SerializeField]
    private TextMeshProUGUI balanceText;   // 유저의 보유금액 표기
    [SerializeField]
    private TextMeshProUGUI tradeText;      // 거래 목록의 총 합계 금액 표기
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

    private void InitPopup()    // 각 탭에 스롯들 생성
    {
        inventory = GameManager.Inst.INVEN;

        // 유저가 상인에게 판매하는 리스트
        for(int i = 0; i < inventory.MAXCounter; i++)
        {
            if(Instantiate(shopSlotPrefab, sellViewContent).TryGetComponent<ShopSlot>(out shopSlot))
            {
                shopSlot.CreateSlot(this, i);   // 슬롯의 인덱스 부여
                shopSlot.gameObject.name = $"SellSlot_{i}";
                // 델리게이트 이벤트 등록
                shopSlot.OnTotalChange += CalculateGold;

                sellSlotList.Add(shopSlot);
            }
        }

        // 상인이 유저에게 판매하는 리스트
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
        // totalGold 계산

        tradeText.text = totalGold.ToString();
        balanceText.text = GameManager.Inst.PlayerGold.ToString();
    }

    public void CalculateGold()
    {
        totalGold = 0;

        if(sellView.activeSelf)     // 판매창이 활성화 되어 있으면
        {
            for(int i = 0; i < sellSlotList.Count; i++)
            {
                if(sellSlotList[i].isActiveAndEnabled)
                {
                    totalGold += sellSlotList[i].TotalGold;
                }
            }
        }
        else   // 구매창이 활성화 되어 있으면
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

    // 인벤토리에 있는 데이터 참조해서 상점 슬롯에 데이터를 갱신
    private void RefreshSellViewData()
    {
        balanceText.text = GameManager.Inst.PlayerGold.ToString();
        dataList = GameManager.Inst.INVEN.GetItemList();    // 최신 데이터 참조

        for(int i = 0; i < inventory.MAXCounter; i++)
        {
            if(i < inventory.CurItemCount && -1 < dataList[i].itemID)      // 아이템이 있는 슬롯
            {
                sellSlotList[i].RefreshSlot(dataList[i]);
            }
            else     // 아이템이 없는 슬롯
            {
                sellSlotList[i].ClearSlot();    // 목록창에서 보이지 않게 비활성화
            }
        }
        totalGold = 0;
        RefreshGold();
    }

    InventoryItemData tempData = new InventoryItemData();
    // 상인의 판매리스트 갱신
    private void RefreshBuyViewData()
    {
        for(int i = 0; i < 4; i++)
        {
            tempData.itemID = 2001001 + i;
            tempData.amount = 999;      // 상점으로부터 한꺼번에 구매할 수 있는 최대 갯수
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
        if(sellView.activeSelf)     // 판매탭 열림
        {
            for(int i = inventory.CurItemCount - 1; i >= 0; i--)
            {
                sellSlotList[i].GetSellCount(out itemID, out tradeCount, out tradeGold);
                GameManager.Inst.PlayerGold += tradeGold;   // 골드 증가

                InventoryItemData tradeData = new InventoryItemData();
                tradeData.itemID = itemID;
                tradeData.amount = tradeCount;
                inventory.DeleteItem(tradeData);    // 아이템 삭제
            }
            OnClick_SellTap();      // 판매탭 갱신
        }
        else     // 구매탭 열림
        {
            totalGold = 0;

            for(int i = 0; i < 4; i++)
            {
                buySlotList[i].GetBuyCount(out itemID, out tradeCount, out tradeGold);
                totalGold += tradeGold;
            }

            if (totalGold <= GameManager.Inst.PlayerGold)    // 플레이어 잔액이 거래하려고 등록한 금액보다 작다면
            {
                GameManager.Inst.PlayerGold -= totalGold;   // 비용 지불

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
            OnClick_BuyTap();   // 상점창 갱신
        }
    }


    public void PopupClose()
    {
        gameObject.LeanScale(Vector3.zero, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }

    public void PopupOpen()
    {
        // 상태값을 갱신
        OnClick_BuyTap();
        gameObject.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }
}
