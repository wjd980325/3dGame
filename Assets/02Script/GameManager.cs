using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임 구동(런타임) 때 테이블 데이터 관리
public class GameManager : Singleton<GameManager>   // 템플릿 문법
{                       // Singleton<T>에 GameManager가 적용됨

    private void Awake()
    {
        base.Awake();

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
}
