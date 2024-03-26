using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportArena : MonoBehaviour
{
    [SerializeField]
    private SceneName targetSceneName = SceneName.TitleScene;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Inst.AsyncLoadNextScene(targetSceneName);
        }
    }

}
