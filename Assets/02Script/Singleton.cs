using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T :MonoBehaviour
{
    private static T instance;
    public static T Inst
    {
        get
        {
            if(instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if(instance == null)    // �ش���� ������ Ÿ���� ��ü�� �ϳ��� ����
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    obj.TryGetComponent<T>(out instance);
                }
            }
            return instance;
        }
    }

    protected void Awake()  // ��ӹ��� �ڽĵ鿡�� �����Ͽ� ȣ���Ҽ� �ְ� protected ���� ������ ���
    {
        // �θ� 1�� �̻� �ִ� ���
        if (transform.parent != null && transform.root != null)
        {
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else  // �θ� ���� ���
            DontDestroyOnLoad(gameObject);
    }
}
