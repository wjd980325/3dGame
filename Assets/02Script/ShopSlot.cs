using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour
{
    // 자신을 소유한 스크립트
    private Popup_Shop popupShop;

    [SerializeField]
    private Image icon;
    [SerializeField]
    private TextMeshProUGUI itemName;
    [SerializeField]
    private TextMeshProUGUI itemPrice;
    [SerializeField]
    private TextMeshProUGUI tradeCount;     // 거래 갯수

    [SerializeField]
    private Button left;
    [SerializeField]
    private Button right;
    [SerializeField]
    private Button max;

    private InventoryItemData data;     // 어떤 아이템? 몇개? 거래할지

    // 몇번째에 있는 인덱스인지
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

    public delegate void TotalGoldChange();     // 대리자
    public event TotalGoldChange OnTotalChange; // 이벤트

    private int tradeGold;      // 1개 거래 금액(단가)
    private int sellMaxCount;   // 거래 가능한 최대 갯수
    private int curCount;       // 거래하려고 등록한 갯수
    private int itemID;

    // 게임 시작할때 슬롯을 만들어줄때 부모, 슬롯의 인덱스 지정
    public void CreateSlot(Popup_Shop _shop, int index)
    {
        popupShop = _shop;
        slotIndex = index;
        icon.enabled = false;
        gameObject.SetActive(false);
    }

    // 닫힌 탭이 열릴때 정보 갱신
    public void RefreshSlot(InventoryItemData newData)
    {
        gameObject.SetActive(true);
        itemID = newData.itemID;
        sellMaxCount = newData.amount;
        curCount = 0;

        // 데이블 데이터가 유효 하다면
        if(GameManager.Inst.GetItemData(itemID, out ItemData_Entity tableItemData))
        {
            // 동적로딩은 컴퓨터에게 부담을 많이 준다
            // 한번 불러온 이미지는 저장(RAM[HeapMemory)을 해놨다가 사용한다
            icon.sprite = Resources.Load<Sprite>(tableItemData.iconImg);    // 동적로딩

            itemPrice.text = tableItemData.sellGold.ToString();
            tradeGold = tableItemData.sellGold;
            tradeCount.text = "0";
            icon.enabled = true;
        }
    }

    // 슬롯 비활성화
    public void ClearSlot()
    {
        gameObject.SetActive(false);
    }

    // 구매하는 아이템의 ID 갯수 총금액 알려주기 위한 메소드
    public bool GetBuyCount(out int _buyItemID, out int _buyItemCount, out int _buyGold)
    {                     //out = 포인터 개념 주소를 가져옴
        _buyItemID = itemID;
        _buyItemCount = curCount;
        _buyGold = totalGold;
        
        //curCount = 0;
        //TotalGold = 0;
        //tradeCount.text = curCount.ToString();
        //TotalGold = curCount * tradeGold;

        // 부모인 ItemShop Popup에 금액을 알려줘야함

        return true;
    }

    // ApplyBtn이 눌렸을때 판매하는 정보 전달
    public bool GetSellCount(out int _sellItemID, out int _sellItemCount, out int _sellGold)
    {
        _sellItemID = itemID;
        _sellItemCount = curCount;
        _sellGold = totalGold;

        //sellMaxCount -= curCount;
        //curCount = 0;
        //tradeCount.text = curCount.ToString();
        //TotalGold = curCount * tradeGold;

        // 부모인 ItemShop Popup에 금액을 알려줘야함

        return true;
    }

    public void OnClickLeftBtn()
    {
        if (curCount > 0)
            curCount--;

        tradeCount.text = curCount.ToString();
        TotalGold = curCount * tradeGold;

        // 부모인 ItemShop Popup에 금액을 알려줘야함
    }
    public void OnClickRightBtn()
    {
        if (sellMaxCount > curCount)
            curCount++;

        tradeCount.text = curCount.ToString();
        TotalGold = curCount * tradeGold;

        // 부모인 ItemShop Popup에 금액을 알려줘야함
    }
    public void OnClickMaxBtn()
    {   // 사과를 50 => 거래가능 갯수도 50
        curCount = sellMaxCount;
        tradeCount.text = curCount.ToString();
        TotalGold = curCount * tradeGold;

        // 부모인 ItemShop Popup에 금액을 알려줘야함
    }

    private void Awake()
    {
        right.onClick.AddListener(OnClickRightBtn);
        left.onClick.AddListener(OnClickLeftBtn);
        max.onClick.AddListener(OnClickMaxBtn);
    }
}
