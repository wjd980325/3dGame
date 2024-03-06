using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraMove : MonoBehaviour
{
    private Transform target;
    [SerializeField]
    private Vector3 offset;
    private GameObject obj;

    private void Awake()
    {
        obj = GameObject.Find("MainChar");
        if (obj != null)
            target = obj.transform;
        else
            Debug.Log("MainCamereMove.cs - Awake() - target ���� ����");
    }

    private void Update()
    {
        transform.position = target.position + offset;
    }
}
