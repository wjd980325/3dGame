using System.Collections;
using System.Collections.Generic;
using System.Linq;  // ��ũ
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ������ ���� ���� ����
// �������� ���纻�� �ٲٸ� ������ ���� �ٲ�
// ������ ���纻�� �ٲ㵵 ������ �ٲ��� ����
public class Popup_Forge : MonoBehaviour, IPopupBase
{
    [SerializeField]
    private GameObject forgeSlotPrefab;
    [SerializeField]
    private RectTransform contentRect;

    [SerializeField]
    private Image iconImg;                      // ������ ������
    [SerializeField]
    private TextMeshProUGUI enchantInfo;        // � -> �
    [SerializeField]
    private TextMeshProUGUI enchantPrice;       // ��ȭ ���
    [SerializeField]
    private TextMeshProUGUI playerBalance;      // �÷��̾� �ܾ�

    [SerializeField]
    private Button tryBtn;

                         // ������ ���� ������
    private List<ForgeSlot> slotList = new List<ForgeSlot>();   // �޸� ���� �Ҵ�

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

    private List<InventoryItemData> dataList;   // �˾�â�� ǥ�õǴ� �����۵��� ����Ʈ
    private ItemData_Entity tableData;      // ���̺� ������ ����
    private InventoryItemData selectItem;   // ���õ� ������ ����

    // �˾��� ������ Ȥ�� �������� ������ ������ �˾�â�� �ֽ������� ����
    private void RefreshData()
    {
        playerBalance.text = GameManager.Inst.PlayerGold.ToString();
        // ������ ����Ʈ ����
        //dataList = inventory.GetItemList();   ���� ����
        dataList = inventory.GetItemList().ToList<InventoryItemData>();     // ���� ����

        // ��ü �����ۿ��� �������� �ƴ� ������ ����
        for(int i = inventory.CurItemCount - 1; i >= 0; i--)
        {
            if(GameManager.Inst.GetItemData(dataList[i].itemID, out tableData))
            {
                if(!tableData.equip)     // �������� �ƴ� ��쿡
                {
                    dataList.RemoveAt(i);   // ���ϴ� �ε����� �ش��ϴ� ������ ����
                }
            }
        }
        // ������ ������ �����ؼ� ������ ǥ��
        for(int i = 0; i < slotList.Count; i++)
        {
            if(i < dataList.Count)  // ǥ���ؾ��ϴ� �������� �ִ� ���
            {
                slotList[i].RefreshSlot(dataList[i]);
            }
            else
            {
                slotList[i].ClearSlot();    // ������ ��Ȱ��ȭ
            }
        }
    }

    // �������� ������ �������� ����������
    // ����â�� �������� �����ϴ� �޼ҵ�
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

                enchantInfo.text = $"��ȭ  {itemData.itemID % 1000} -> {(itemData.itemID % 1000) + 1}";
                enchantPrice.text = $"��ȭ ��� {itemData.itemID % 1000 * 500}";
                playerBalance.text = $"���� �ݾ� {GameManager.Inst.PlayerGold}";
            }
            slotList[i].IsFocus = false;    // ���õ��� ���� �������� ���� ������ ��Ȱ��ȭ
        }
    }

    public void OnClickEnchant()
    {
        if(TryEnchant())    // ����
        {
            selectItem.itemID += 1;
            GameManager.Inst.INVEN.UpdateItemInfo(selectItem);
            SeletItem(selectItem);      // ����â ����

        }
        else       // ����
        {

        }
    }

    private bool TryEnchant()       // ���� ��ȭ �õ��ϰ� ���� ���θ� ����
    {
        bool isSuccess = false;

        if(CanEnchant())
        {
            isSuccess = Random.Range(0, 10001) < 9000;  // ���� Ȯ�� 90%
            GameManager.Inst.PlayerGold -= ((selectItem.itemID % 1000) * 500);
            RefreshData();      // �˾� â ����
        }
        return isSuccess;
    }

    private bool CanEnchant()       // ��ȭ �õ��� ������ �������� üũ
    {
        if(selectItem.itemID % 1000 >= 5)   // �ִ�ġ���� ��ȭ ���� ������
        {
            return false;
        }
        if(selectItem.itemID % 1000 * 500 > GameManager.Inst.PlayerGold)    // ��ȭ ��� ����
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
        // �κ��丮 ������ ������� ����
        RefreshData();
        gameObject.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeOutElastic);
    }
}
