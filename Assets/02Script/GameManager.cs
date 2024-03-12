using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // JSON
    #region _Save&Load_

    public void SaveData()  // ���Ϸ� �߿䵥���� ����
    {

    }
    public void LoadData()  // ���� �о ��Ÿ�� �Ľ�
    {

    }

    #endregion
}
