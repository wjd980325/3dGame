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

    // �ܼ� ���� ������ Ÿ�ֿ̹� ������ �̸� �����صδ� ����
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

    // ������� �������� ���Ͽ� ȭ�鿡 �κ��丮�� �����ִ� �Լ�
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
                slotList[i].ClearSlot();    // ��ĭó��
            }
            slotList[i].SetSelectSlot(false);   // ���õ��� ���� �������� ����
        }
    }

}
