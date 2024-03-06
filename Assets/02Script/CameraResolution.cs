using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private void Awake()
    {
        if(TryGetComponent<Camera>(out Camera cam))
        {
            Rect rt = cam.rect;     // �ν�����â ����Ʈ ��Ʈ
                                    // ������� �ػ󵵸� �ȼ��� �޾ƿ�
            float scale_height = ((float)Screen.width / Screen.height) / ((float)16 / 9);
            float scale_width = 1f / scale_height;

            if(scale_height < 1f)
            {
                rt.height = scale_height;
                rt.y = (1f - scale_height) / 2f;
            }
            else
            {
                rt.width = scale_width;
                rt.x = (1f - scale_width) / 2f;
            }
            cam.rect = rt;
        }
    }
}
