using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateTest : MonoBehaviour
{
    // ��������Ʈ�� ������ ����
    public delegate void TotalGoldChange(); // �Լ� ������

    public event TotalGoldChange onTotalChange;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            onTotalChange += HelloTotal;

        if (Input.GetKeyDown(KeyCode.S))
            onTotalChange += WelcomeTotal;

        if (Input.GetKeyDown(KeyCode.Space))
            onTotalChange();
    }

    public void HelloTotal()
    {
        Debug.Log("�ȳ��ϼ���.");
    }

    public void WelcomeTotal()
    {
        Debug.Log("ȯ���մϴ�.");
    }
}
