using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 관찰자(Observer) 입장에서 플레이어가 발생시킨 Noti를 받기위해 만들어지는 인터페이스
public interface IObserverUI
{
    void UpdateUI(MyCharState charState);
}

// 데이터의 주체(Subject)
public interface ISubjectUI
{
    void Attach(IObserverUI observer);  // 구독자가 구독을 신청하면 받아들이는

    void Detach(IObserverUI observer); // 구독에서 제외시키는 메소드

    void Notify();  // 구독자에게 알림을 보내는 함수
}