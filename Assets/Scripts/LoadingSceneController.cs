using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public struct LoadingInfo
{
    [SerializeField]
    public string nextScene;

    [Range(0.01f, 3f)]
    public float speed;

    [Range(0.01f, 3f)]
    public float finalSpeed;
}

public class LoadingSceneController : MonoBehaviour
{
    public LoadingInfo loadingInfo;
    [SerializeField] private Slider loadingSlider;

    void Start()
    {
        if (GameManager.instance)
        {
            loadingInfo.nextScene = GameManager.instance.targetScene;
            GameManager.instance.targetScene = string.Empty;
        }
        else
        {
            Debug.LogWarning("GameManager가 없습니다.", gameObject);
        }

        if (loadingInfo.nextScene == string.Empty)
        {
            Debug.LogError("이동할 씬 정보가 없습니다.", gameObject);
            return;
        }

        StartCoroutine(LoadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine()
    {
        AsyncOperation aop = SceneManager.LoadSceneAsync(loadingInfo.nextScene);
        aop.allowSceneActivation = false;

        while (!aop.isDone)
        {
            // ~ 90% 속도 조절
            if (loadingSlider.value <= (0.899f))
            {
                loadingSlider.value += Time.deltaTime * loadingInfo.speed;
                if (loadingSlider.value > aop.progress)
                {
                    loadingSlider.value = aop.progress;
                }
            }
            // 90% ~ 속도 조절
            else
            {
                loadingSlider.value += Time.deltaTime * loadingInfo.finalSpeed;
                if (loadingSlider.value >= 1f)
                {
                    aop.allowSceneActivation = true;
                    yield break;
                }
            }

            yield return null;
        }
    }
}
