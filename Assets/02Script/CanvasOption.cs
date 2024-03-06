using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasOption : MonoBehaviour
{
    private void Awake()
    {
        if(TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
        }
    }
}
