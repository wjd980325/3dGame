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
                buySlotList.Add(shopSlot);
            }
        }

        sellTapBtn.onClick.AddListener(OnClick_SellTap);
        buyTapBtn.onClick.AddListener(OnClick_BuyTap);
        tradeBtn.onClick.AddListener(OnClick_ApplyBtn);
    }

    private int totalGold;

    public void RefreshGold()
    {
        // totalGold 계산

        tradeText.text = totalGold.ToString();
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
        tradeText.text = "0";
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
    public void OnClick_ApplyBtn()
    {

    }


    public void PopupClose()
    {
        gameObject.LeanScale(Vector3.zero, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }

    public void PopupOpen()
    {
        // 상태값을 갱신
        RefreshBuyViewData();
        gameObject.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }
}
