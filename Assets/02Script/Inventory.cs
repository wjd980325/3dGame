using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItemData
{
    public int uid;     // �������� ���� ID
    public int itemID;  // ���̺��� ItemID
    public int amount;  // ������ ����
}

[System.Serializable]   // ����ȭ
public class Inventory
{
    private int maxItemCount = 18;  // �κ��丮�� ������ �ִ� �������� ����
    public int MAXCounter => maxItemCount;

    private int curItemCount;
    public int CurItemCount
    {
        get => curItemCount;
        set => curItemCount = value;
    }

    [SerializeField]    // �����뵵
    private List<InventoryItemData> items = new List<InventoryItemData>();

    // �κ��丮�� �������� �߰�
    public void AddItem(InventoryItemData newItem)
    {
        int index = FindItemIndex(newItem);     // �κ��丮 ���� ������ Ÿ���� �������� �ִ���

        if(true)    // ������ ���̺� �ش� �����ۿ� ���� ������ �ִ���
        {
            if(true)    // ������ ������ ���������� üũ
            {
                newItem.uid = 1;    // ��ġ�� �ʴ� UID ����
                newItem.amount = 1;
                items.Add(newItem);     // �������� ������ �߰�
                curItemCount++;     // ��� ���� ����
            }
            else if(-1 < index)     // ��ø�� �����ϰ� �̹� ������ �ִ� ������
            {
                items[index].amount += newItem.amount;
            }
            else  // ��ø�� �����ϰ� ó�� ������ ������
            {
                newItem.uid = -1;
                items.Add(newItem);
                curItemCount++;
            }
        }
    }

    // ������ ��ȭ ������ ���� ����
    public void UpdateItemInfo(InventoryItemData newData)
    {
        for(int i =0; i < items.Count; i++)
        {
            if(items[i].uid == newData.uid)
            {
                Debug.Log(items[i].itemID + " ��ȭ ó�� " + items[i].uid);
                items[i].itemID = newData.itemID;
                items[i].amount = newData.amount;
                
            }
        }
    }

    // �κ��丮 �������ִ��� Ȯ��
    public bool IsFull()
    {
        return curItemCount >= maxItemCount;
    }

    // �� �κ��丮�� �̹� �ߺ��� �������� �ִٸ� ���° ���Կ� �ִ��� index�� ����
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

    // �κ��丮 �ܺ� ����
    public List<InventoryItemData> GetItemList()
    {
        CurItemCount = items.Count;     // ������ ������ ���� ����
        return items;
    }

    // ������ �Ȱų� �Ҹ������� �������� ����
    public void DeleteItem(InventoryItemData deleteItem)
    {
        int index = FindItemIndex(deleteItem);
        if(index > -1)
        {
            items[index].amount -= deleteItem.amount;   // ���� ���� ����
            if(items[index].amount < 1f)
            {
                items.RemoveAt(index);
                curItemCount--;
            }
        }
    }
}
