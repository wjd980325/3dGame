using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateTest : MonoBehaviour
{
    // 델리게이트의 형식을 선언
    public delegate void TotalGoldChange(); // 함수 포인터

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
        Debug.Log("안녕하세요.");
    }

    public void WelcomeTotal()
    {
        Debug.Log("환영합니다.");
    }
}
