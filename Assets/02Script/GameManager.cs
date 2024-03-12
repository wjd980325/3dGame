using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // JSON
    #region _Save&Load_

    public void SaveData()  // 파일로 중요데이터 저장
    {

    }
    public void LoadData()  // 파일 읽어서 런타임 파싱
    {

    }

    #endregion
}
