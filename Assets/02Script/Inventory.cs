using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItemData
{
    public int uid;     // 아이템의 고유 ID
    public int itemID;  // 테이블의 ItemID
    public int amount;  // 아이템 갯수
}

[System.Serializable]   // 직렬화
public class Inventory
{
    private int maxItemCount = 18;  // 인벤토리에 담을수 있는 아이템의 종류
    public int MAXCounter => maxItemCount;

    private int curItemCount;
    public int CurItemCount
    {
        get => curItemCount;
        set => curItemCount = value;
    }

    [SerializeField]    // 디버깅용도
    private List<InventoryItemData> items = new List<InventoryItemData>();

    // 인벤토리에 아이템을 추가
    public void AddItem(InventoryItemData newItem)
    {
        int index = FindItemIndex(newItem);     // 인벤토리 내에 동일한 타입의 아이템이 있는지

        if(true)    // 데이터 테이블에 해당 아이템에 대한 정보가 있는지
        {
            if(true)    // 장착이 가능한 아이템인지 체크
            {
                newItem.uid = 1;    // 겹치지 않는 UID 생성
                newItem.amount = 1;
                items.Add(newItem);     // 실질적인 아이템 추가
                curItemCount++;     // 사용 슬롯 증가
            }
            else if(-1 < index)     // 중첩이 가능하고 이미 가지고 있는 아이템
            {
                items[index].amount += newItem.amount;
            }
            else  // 중첩이 가능하고 처음 습득한 아이템
            {
                newItem.uid = -1;
                items.Add(newItem);
                curItemCount++;
            }
        }
    }

    // 아이템 강화 아이템 정보 갱신
    public void UpdateItemInfo(InventoryItemData newData)
    {
        for(int i =0; i < items.Count; i++)
        {
            if(items[i].uid == newData.uid)
            {
                Debug.Log(items[i].itemID + " 강화 처리 " + items[i].uid);
                items[i].itemID = newData.itemID;
                items[i].amount = newData.amount;
                
            }
        }
    }

    // 인벤토리 가득차있는지 확인
    public bool IsFull()
    {
        return curItemCount >= maxItemCount;
    }

    // 현 인벤토리에 이미 중복된 아이템이 있다면 몇번째 슬롯에 있는지 index를 리턴
    private int FindItemIndex(InventoryItemData newItem)
    {
        for(int i = items.Count -1; i >= 0; i--)
        {
            if(items[i].itemID == newItem.itemID)
            {
                return i;
            }
        }
        return -1;
    }

    // 인벤토리 외부 참조
    public List<InventoryItemData> GetItemList()
    {
        CurItemCount = items.Count;     // 보유한 아이템 갯수 갱신
        return items;
    }

    // 상점에 팔거나 소모했을때 아이템을 제거
    public void DeleteItem(InventoryItemData deleteItem)
    {
        int index = FindItemIndex(deleteItem);
        if(index > -1)
        {
            items[index].amount -= deleteItem.amount;   // 보유 갯수 감소
            if(items[index].amount < 1f)
            {
                items.RemoveAt(index);
                curItemCount--;
            }
        }
    }
}
