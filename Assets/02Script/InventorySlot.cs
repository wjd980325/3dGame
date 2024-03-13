using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ������ sprite(ICON) ����
// ���� �������� ����
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

    // �ش��ϴ� ������ ������ ������ �̹����� Amount�� �������ִ� �޼ҵ�
    public void DrawItemSlot(InventoryItemData itemData)
    {
        if(GameManager.Inst.GetItemData(itemData.itemID, out ItemData_Entity data))
        {
            // ���� �ε��� ���ؼ� ������ ����
            //icon.sprite = 
            icon.enabled = true;
            ChangeAmount(itemData.amount);
            isEmpty = false;

        }
    }

    // �����ߴ� �������� ���������� ������ ��ĭ���� �������ִ� �޼ҵ�
    public void ClearSlot()
    {

    }

    // ��ø �������� ���� ������ �������ִ� �޼ҵ�
    public void ChangeAmount(int newAmount)
    {

    }

    // ���� ���� ���θ� �����ϴ� �Լ�
    public void SetSelectSlot(bool isSelect)
    {

    }

    public void OnClick_Select()
    {

    }
}
