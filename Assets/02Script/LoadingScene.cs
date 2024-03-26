using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    private Image loadingBar;
    [SerializeField]
    private TextMeshProUGUI tipText;

    private void Awake()
    {
        loadingBar.fillAmount = 0f;
        StartCoroutine("LoadAsyncScene");     // 다음씬 비동기 로딩
    }

    IEnumerator LoadAsyncScene()
    {
        yield return null;
        tipText.text = GameManager.Inst.GetTipMessage(GameManager.Inst.NextScene);
        yield return YieldInstructionCache.WaitForSeconds(3f);
        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(GameManager.Inst.NextScene.ToString());
        asyncScene.allowSceneActivation = false;
        float timeC = 0f;
        while(!asyncScene.isDone)
        {
            yield return null;
            timeC += Time.deltaTime;
            if (asyncScene.progress >= 0.9f)
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1f, timeC);
                if (loadingBar.fillAmount >= 0.999f)
                    asyncScene.allowSceneActivation = true;
            }
            else
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, asyncScene.progress, timeC);
                if (loadingBar.fillAmount >= asyncScene.progress)
                    timeC = 0f;
            }
        }


        // 다음씬에 따라서 팁 메세지 변경
    }
}
