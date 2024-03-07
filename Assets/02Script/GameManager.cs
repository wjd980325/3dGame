using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ����(��Ÿ��) �� ���̺� ������ ����
public class GameManager : Singleton<GameManager>   // ���ø� ����
{                       // Singleton<T>�� GameManager�� �����

    private void Awake()
    {
        base.Awake();

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
}
