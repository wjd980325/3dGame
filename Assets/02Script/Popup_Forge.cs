using System.Collections;
using System.Collections.Generic;
using System.Linq;  // 링크
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 참조형 변수 값형 변수
// 참조형은 복사본을 바꾸면 원본도 같이 바뀜
// 값형은 복사본을 바꿔도 원본이 바뀌지 않음
public class Popup_Forge : MonoBehaviour, IPopupBase
{
    [SerializeField]
    private GameObject forgeSlotPrefab;
    [SerializeField]
    private RectTransform contentRect;

    [SerializeField]
    private Image iconImg;                      // 아이템 아이콘
    [SerializeField]
    private TextMeshProUGUI enchantInfo;        // 몇강 -> 몇강
    [SerializeField]
    private TextMeshProUGUI enchantPrice;       // 강화 비용
    [SerializeField]
    private TextMeshProUGUI playerBalance;      // 플레이어 잔액

    [SerializeField]
    private Button tryBtn;

                         // 참조형 변수 포인터
    private List<ForgeSlot> slotList = new List<ForgeSlot>();   // 메모리 동적 할당

    private Inventory inventory;

    private ForgeSlot slot;

    private void Awake()
    {
        InitPopup();
        PopupClose();
    }

    private void InitPopup()
    {
        inventory = GameManager.Inst.INVEN;

        for (int i = 0; i < inventory.MAXCounter; i++)
        {
            if(Instantiate(forgeSlotPrefab, contentRect).TryGetComponent<ForgeSlot>(out slot))
            {
                slot.gameObject.name = "ForgeSlot_" + i;
                slot.CreateSlot();
                slot.OnSelectData += SeletItem;
                slotList.Add(slot);
            }
        }

        tryBtn.onClick.AddListener(OnClickEnchant);
    }

    private List<InventoryItemData> dataList;   // 팝업창에 표시되는 아이템들의 리스트
    private ItemData_Entity tableData;      // 테이블 데이터 참조
    private InventoryItemData selectItem;   // 선택된 아이템 참조

    // 팝업이 열릴때 혹은 아이템의 변동이 있을때 팝업창을 최신정보로 갱신
    private void RefreshData()
    {
        playerBalance.text = GameManager.Inst.PlayerGold.ToString();
        // 아이템 리스트 복사
        //dataList = inventory.GetItemList();   얕은 복사
        dataList = inventory.GetItemList().ToList<InventoryItemData>();     // 깊은 복사

        // 전체 아이템에서 장착템이 아닌 아이템 제거
        for(int i = inventory.CurItemCount - 1; i >= 0; i--)
        {
            if(GameManager.Inst.GetItemData(dataList[i].itemID, out tableData))
            {
                if(!tableData.equip)     // 장착템이 아닌 경우에
                {
                    dataList.RemoveAt(i);   // 원하는 인덱스에 해당하는 데이터 삭제
                }
            }
        }
        // 슬롯의 정보를 전달해서 슬롯을 표시
        for(int i = 0; i < slotList.Count; i++)
        {
            if(i < dataList.Count)  // 표시해야하는 아이템이 있는 경우
            {
                slotList[i].RefreshSlot(dataList[i]);
            }
            else
            {
                slotList[i].ClearSlot();    // 슬롯의 비활성화
            }
        }
    }

    // 오른쪽의 슬롯의 아이템을 선택했을때
    // 왼쪽창의 정보만을 갱신하는 메소드
    public void SeletItem(InventoryItemData itemData)
    {
        for(int i = 0; i < dataList.Count; i++)
        {
            if(dataList[i].uid == itemData.uid)
            {
                selectItem = itemData;
                if(GameManager.Inst.GetItemData(itemData.itemID, out tableData))
                {
                    iconImg.enabled = true;
                    iconImg.sprite = Resources.Load<Sprite>(tableData.iconImg);
                }
                else
                {
                    iconImg.enabled = false;
                }

                enchantInfo.text = $"강화  {itemData.itemID % 1000} -> {(itemData.itemID % 1000) + 1}";
                enchantPrice.text = $"강화 비용 {itemData.itemID % 1000 * 500}";
                playerBalance.text = $"보유 금액 {GameManager.Inst.PlayerGold}";
            }
            slotList[i].IsFocus = false;    // 선택되지 않은 아이템의 선택 아이콘 비활성화
        }
    }

    public void OnClickEnchant()
    {
        if(TryEnchant())    // 성공
        {
            selectItem.itemID += 1;
            GameManager.Inst.INVEN.UpdateItemInfo(selectItem);
            SeletItem(selectItem);      // 왼쪽창 갱신

        }
        else       // 실패
        {

        }
    }

    private bool TryEnchant()       // 실제 강화 시도하고 성공 여부를 리턴
    {
        bool isSuccess = false;

        if(CanEnchant())
        {
            isSuccess = Random.Range(0, 10001) < 9000;  // 성공 확률 90%
            GameManager.Inst.PlayerGold -= ((selectItem.itemID % 1000) * 500);
            RefreshData();      // 팝업 창 갱신
        }
        return isSuccess;
    }

    private bool CanEnchant()       // 강화 시도가 가능한 상태인지 체크
    {
        if(selectItem.itemID % 1000 >= 5)   // 최대치까지 강화 끝난 아이템
        {
            return false;
        }
        if(selectItem.itemID % 1000 * 500 > GameManager.Inst.PlayerGold)    // 강화 비용 부족
        {
            return false;
        }
        return true;
    }

    public void PopupClose()
    {
        gameObject.LeanScale(Vector3.zero, 0.7f).setEase(LeanTweenType.easeOutElastic);
    }

    public void PopupOpen()
    {
        // 인벤토리 데이터 기반으로 갱신
        RefreshData();
        gameObject.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeOutElastic);
    }
}
