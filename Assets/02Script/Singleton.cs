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
                if(instance == null)    // 해당씬에 동일한 타입의 객체가 하나도 없다
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    obj.TryGetComponent<T>(out instance);
                }
            }
            return instance;
        }
    }

    protected void Awake()  // 상속받은 자식들에서 접급하여 호출할수 있게 protected 접근 제한자 사용
    {
        // 부모가 1개 이상 있는 경우
        if (transform.parent != null && transform.root != null)
        {
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else  // 부모가 없는 경우
            DontDestroyOnLoad(gameObject);
    }
}
