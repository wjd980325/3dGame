using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;    // 파일 입출력

[System.Serializable]
public class PlayerData     // 유저가 게임 내에서 만들어낸 데이터 중에서 저장해야되는 데이터들
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

// 게임 구동(런타임) 때 테이블 데이터 관리
// 인벤토리 데이터 관리
// 저장, 로드 기능
public class GameManager : Singleton<GameManager>   // 템플릿 문법
{                       // Singleton<T>에 GameManager가 적용됨
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

        // 테이블 동적 로딩하여 자료구조에 맞게 데이터 파싱
        #region _TableData_

        table = Resources.Load<ActionGame>("ActionGame");

        // 빠른 탐색을 위해서 Dictionary 자료구조로 옮김
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
            return -1;  // 테이블 없음
        }
        return table.LevelEXP[curLevel - 1].nextEXP;
    }


    // JSON
    #region _Save&Load_
    private string dataPath;
    public void SaveData()  // 파일로 중요데이터 저장
    {
        string data = JsonUtility.ToJson(pData);
        File.WriteAllText(dataPath, data);
    }
    public bool LoadData()  // 파일 읽어서 런타임 파싱
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
        get => pData.gold;  // 읽기 가능
        set => pData.gold = value;  // 쓰기 가능
    }
    public string PlayerName
    {
        get => pData.userNickName;  // 읽기 가능
    }
    public int PlayerLevel => pData.level;
    public int PlayerCurrentEXP => pData.curEXP;
    public void AddEXP(int addEXP)
    {
        pData.curEXP += addEXP;
        if(pData.curEXP >= GetNextEXPData(pData.level))    // 레벨업이 가능하다면
        {
            LevelUPProcess();
        }

        // UI 갱신처리
    }

    private void LevelUPProcess()
    {
        pData.curEXP -= GetNextEXPData(pData.level);
        pData.level++;
        pData.maxHP += Random.Range(5, 10);
        pData.maxMP += Random.Range(10, 20);
    }



    #endregion

    // 아이템 습득 처리 해주는 함수
    public bool LootingItem(InventoryItemData newItem)
    {
        if(!pData.inventory.IsFull())
        {
            pData.inventory.AddItem(newItem);

            Debug.Log($"인벤토리 내 아이템 갯수 : {pData.inventory.CurItemCount}");
            return true;
        }
        return false;
    }
}
