using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;    // ���� �����

[System.Serializable]
public class PlayerData     // ������ ���� ������ ���� ������ �߿��� �����ؾߵǴ� �����͵�
{
    public string userNickName;
    public int level;
    public int curEXP;
    public int curHP;
    public int maxHP;
    public int curMP;
    public int maxMP;
    public int gold;
    public int uidCounter;
    public Inventory inventory;
}

// ���� ����(��Ÿ��) �� ���̺� ������ ����
// �κ��丮 ������ ����
// ����, �ε� ���
public class GameManager : Singleton<GameManager>   // ���ø� ����
{                       // Singleton<T>�� GameManager�� �����
    private PlayerData pData;
    public PlayerData PlayerData
    {
        get => pData;
    }

    private void Awake()
    {
        base.Awake();

        pData = new PlayerData();
        dataPath = Application.persistentDataPath + "/Save";


        CreateUserData("aaa");

        // ���̺� ���� �ε��Ͽ� �ڷᱸ���� �°� ������ �Ľ�
        #region _TableData_

        table = Resources.Load<ActionGame>("ActionGame");

        // ���� Ž���� ���ؼ� Dictionary �ڷᱸ���� �ű�
        for(int i = 0; i < table.ItemData.Count; i++)
        {
            dicItemData.Add(table.ItemData[i].id, table.ItemData[i]);
        }

        for (int i = 0; i < table.MonsterData.Count; i++)
        {
            dicMonsterData.Add(table.MonsterData[i].id, table.MonsterData[i]);
        }
        #endregion
    }
    private ActionGame table;
    private Dictionary<int, ItemData_Entity> dicItemData = new Dictionary<int, ItemData_Entity>();
    private Dictionary<int, MonsterData_Entity> dicMonsterData = new Dictionary<int, MonsterData_Entity>();

    public bool GetItemData(int itemID, out ItemData_Entity data)
    {
        return dicItemData.TryGetValue(itemID, out data);
    }

    public bool GetMonsterData(int monsterID, out MonsterData_Entity data)
    {
        return dicMonsterData.TryGetValue(monsterID, out data);
    }

    public int GetNextEXPData(int curLevel)
    {
        if(table.LevelEXP.Count < curLevel)
        {
            return -1;  // ���̺� ����
        }
        return table.LevelEXP[curLevel - 1].nextEXP;
    }


    // JSON
    #region _Save&Load_
    private string dataPath;
    public void SaveData()  // ���Ϸ� �߿䵥���� ����
    {
        string data = JsonUtility.ToJson(pData);
        File.WriteAllText(dataPath, data);
    }
    public bool LoadData()  // ���� �о ��Ÿ�� �Ľ�
    {
        if(File.Exists(dataPath))
        {
            string data = File.ReadAllText(dataPath);
            pData = JsonUtility.FromJson<PlayerData>(data);
            return true;
        }
        return false;
    }
    public void DeleteData()
    {
        File.Delete(dataPath);
    }

    public bool CheckData()
    {
        if(File.Exists(dataPath))
        {
            return LoadData();
        }
        return false;
    }

    #endregion

    #region _UserData_

    public void CreateUserData(string newNickName)
    {
        pData.userNickName = newNickName;
        pData.curEXP = 0;
        pData.gold = 5000;
        pData.level = 1;
        pData.curHP = pData.maxHP = 50;
        pData.curMP = pData.maxMP = 30;
        pData.uidCounter = 0;
        pData.inventory = new Inventory();
    }

    #endregion

    #region _PlayerDataGetter_
    public Inventory INVEN
    {
        get => pData.inventory;
    }
    public int PlayerUIDMaker
    {
        get
        {
            return ++pData.uidCounter;
        }
    }
    public int PlayerGold
    {
        get => pData.gold;  // �б� ����
        set => pData.gold = value;  // ���� ����
    }
    public string PlayerName
    {
        get => pData.userNickName;  // �б� ����
    }
    public int PlayerLevel => pData.level;
    public int PlayerCurrentEXP => pData.curEXP;
    public void AddEXP(int addEXP)
    {
        pData.curEXP += addEXP;
        if(pData.curEXP >= GetNextEXPData(pData.level))    // �������� �����ϴٸ�
        {
            LevelUPProcess();
        }

        // UI ����ó��
    }

    private void LevelUPProcess()
    {
        pData.curEXP -= GetNextEXPData(pData.level);
        pData.level++;
        pData.maxHP += Random.Range(5, 10);
        pData.maxMP += Random.Range(10, 20);
    }



    #endregion

    // ������ ���� ó�� ���ִ� �Լ�
    public bool LootingItem(InventoryItemData newItem)
    {
        if(!pData.inventory.IsFull())
        {
            pData.inventory.AddItem(newItem);

            Debug.Log($"�κ��丮 �� ������ ���� : {pData.inventory.CurItemCount}");
            return true;
        }
        return false;
    }
}
