using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharState : MonoBehaviour, ISubjectUI
{
    private float currentHP;
    public float CurrentHP
    {
        get => currentHP;
        set
        {
            // 범위 제한 0 < 값 < maxHP
            currentHP = Mathf.Clamp(value, 0f, maxHP);
            Notify();   // 알람울리기 옵저버들에게 알람 전달
        }
    }
    private float maxHP;
    public float MaxHP => maxHP;    // 읽기전용 변수

    private float currentMP;
    public float CurrentMP
    {
        get => currentMP;
        set
        {
            currentMP = Mathf.Clamp(value, 0f, maxMP);
            Notify();   // 알람울리기 옵저버들에게 알람 전달
        }
    }
    private float maxMP;
    public float MaxMP => maxMP;    // 읽기전용 변수

    // 옵저버들을 관리하기 위한 목록화
    private List<IObserverUI> observerUIs = new List<IObserverUI>();

    public void Attach(IObserverUI observer)
    {
        observerUIs.Add(observer);
    }

    public void Detach(IObserverUI observer)
    {
        observerUIs.Remove(observer);
    }

    public void Notify()
    {
        for(int i = 0; i < observerUIs.Count; i++)
        {
            observerUIs[i].UpdateUI(this);
        }
    }

    // 테스트
    private void Awake()
    {
        CurrentHP = maxHP = 250f;
        CurrentMP = maxMP = 100f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            CurrentHP += 10f;
        if (Input.GetKeyDown(KeyCode.F2))
            CurrentHP -= 10f;
        if (Input.GetKeyDown(KeyCode.F3))
            CurrentMP += 10f;
        if (Input.GetKeyDown(KeyCode.F4))
            CurrentMP -= 10f;


    }
}
