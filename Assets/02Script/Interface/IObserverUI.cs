using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������(Observer) ���忡�� �÷��̾ �߻���Ų Noti�� �ޱ����� ��������� �������̽�
public interface IObserverUI
{
    void UpdateUI(MyCharState charState);
}

// �������� ��ü(Subject)
public interface ISubjectUI
{
    void Attach(IObserverUI observer);  // �����ڰ� ������ ��û�ϸ� �޾Ƶ��̴�

    void Detach(IObserverUI observer); // �������� ���ܽ�Ű�� �޼ҵ�

    void Notify();  // �����ڿ��� �˸��� ������ �Լ�
}