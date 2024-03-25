using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubScript : MonoBehaviour
{
    DelegateTest test01;

    private void Awake()
    {
        test01 = GameObject.FindAnyObjectByType<DelegateTest>();
        test01.onTotalChange += TestEvent;
    }

    public void TestEvent()
    {
        Debug.Log("서브 시스템");
    }
}
