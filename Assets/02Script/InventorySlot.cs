using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 슬롯의 sprite(ICON) 변경
// 소유 아이템의 갯수
public class InventorySlot : MonoBehaviour
{
    private bool isEmpty;
    public bool EMPTY
    {
        get => isEmpty;
    }

    private int slotIndex;
    public int SLOTIndex
    {
        get => slotIndex;
        set => slotIndex = value;
    }

    private Image icon;
    private GameObject focus;
    private TextMeshProUGUI amount;
    private Button button;
    private bool isSelect;

    private void Awake()
    {
        transform.GetChild(0).TryGetComponent<Image>(out icon);
        focus = transform.GetChild(1).gameObject;
        transform.GetChild(2).TryGetComponent<TextMeshProUGUI>(out amount);
        if (TryGetComponent<Button>(out button))
            button.onClick.AddListener(OnClick_Select);

        ClearSlot();
    }

    // 해당하는 아이템 정보로 슬롯의 이미지와 Amount를 갱신해주는 메소드
    public void DrawItemSlot(InventoryItemData itemData)
    {
        if(GameManager.Inst.GetItemData(itemData.itemID, out ItemData_Entity data))
        {
            // 동적 로딩을 통해서 아이콘 변경
            //icon.sprite = 
            icon.enabled = true;
            ChangeAmount(itemData.amount);
            isEmpty = false;

        }
    }

    // 소유했던 아이템이 없어졌을때 슬롯을 빈칸으로 변경해주는 메소드
    public void ClearSlot()
    {

    }

    // 중첩 아이템의 보유 갯수를 변경해주는 메소드
    public void ChangeAmount(int newAmount)
    {

    }

    // 슬롯 선택 여부를 변경하는 함수
    public void SetSelectSlot(bool isSelect)
    {

    }

    public void OnClick_Select()
    {

    }
}
