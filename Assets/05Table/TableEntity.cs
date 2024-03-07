using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData_Entity
{
    public int id;
    public string name;
    public string iconImg;
    public int sellGold;
    public int attackDamage;
    public int attackRange;
    public int attackRate;
    public bool equip;
}

[System.Serializable]
public class TipData_Entity
{
    public int id;
    public string sceneName;
    public string tipText;
}

[System.Serializable]
public class Languege_Entity
{
    public int id;
    public string KOR;
    public string ENG;
}

[System.Serializable]
public class MonsterData_Entity
{
    public int id;
    public string monsterName;
    public int monsterType;
    public int moveSpeed;
    public int attackDamage;
    public int maxHP;
    public int dropID;
    public int dropRate;
    public int EXP;
}

[System.Serializable]
public class EXP_Entity
{
    public int curLevel;
    public int nextEXP;
}
