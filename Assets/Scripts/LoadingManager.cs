using JetBrains.Annotations;
using Sirenix.OdinInspector;
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

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;

    [Title("Important")]
    public LoadingInfo loadingInfo;
    [SerializeField] private Slider loadingSlider;
    public bool allowLoading = true;

    [Title("Character Sprites")]
    public List<Sprite[]> sprites;
    public Sprite[] walkSprites;
    public Sprite[] runSprites;
    public Sprite[] winSprites;

    void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
        #endregion
        print("List 초기화");
        sprites = new List<Sprite[]>
        {
            walkSprites,
            runSprites,
            winSprites
        };
    }

    void Start()
    {
        if (!allowLoading) return;

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
        loadingSlider.value = 0;

        AsyncOperation aop = SceneManager.LoadSceneAsync(loadingInfo.nextScene);
        aop.allowSceneActivation = false;

        while (!aop.isDone)
        {
            // ~ 90% 속도 조절
            if (loadingSlider.value <= 0.895f)
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
