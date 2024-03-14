using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private ScrollRect scrollRect;

    [SerializeField]
    private GameObject slotPrefab;

    [SerializeField]
    private RectTransform contentTrans;

    private List<InventorySlot> slotList = new List<InventorySlot>();

    private InventorySlot slot;

    private int currentCount;
    private int maxCount;

    private List<InventoryItemData> dataList;

    private void Awake()
    {
        InitSlot();
    }

    // 단순 세임 시작할 타이밍에 슬롯을 미리 생성해두는 역할
    private void InitSlot()
    {
        for(int i = 0; i < 18; i++)
        {
            if(Instantiate(slotPrefab, contentTrans).TryGetComponent<InventorySlot>(out slot))
            {
                slot.SLOTIndex = i;
                slotList.Add(slot);
            }
        }
    }

    // 사용자의 조작으로 인하여 화면에 인벤토리를 보여주는 함수
    public void RefreshInventoryUI()
    {
        dataList = GameManager.Inst.INVEN.GetItemList();
        currentCount = GameManager.Inst.INVEN.CurItemCount;
        maxCount = GameManager.Inst.INVEN.MAXCounter;

        for(int i = 0; i < maxCount; i++)
        {
            if(i < currentCount && dataList[i].itemID > -1)
            {
                slotList[i].DrawItemSlot(dataList[i]);
            }
            else
            {
                slotList[i].ClearSlot();    // 빈칸처리
            }
            slotList[i].SetSelectSlot(false);   // 선택되지 않은 슬록으로 지정
        }
    }

}
