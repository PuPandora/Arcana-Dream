using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GetItemUI : MonoBehaviour
{
    [SerializeField] private DOTweenAnimation tween;
    Image itemIcon;
    TextMeshProUGUI itemInfoText;

    void Awake()
    {
        itemIcon = GetComponentsInChildren<Image>(true)[1];
        itemInfoText = GetComponentInChildren<TextMeshProUGUI>();

        gameObject.SetActive(false);
    }

    private void Assign()
    {
        // 비효율적인 것 같아 에디터에서 직접 할당

        // 모든 아이템 획득 UI가 접근하는 것을 방지 (성능 낭비 방지)
        if (StageManager.instance.getItemUis == null)
        {
            StageManager.instance.getItemUis = transform.parent.parent.GetComponentsInChildren<GetItemUI>();
            var parents = transform.parent.parent.GetComponentsInChildren<Transform>();
            var uiArray = new GetItemUI[parents.Length];

            for (int i = 0; i < parents.Length; i++)
            {
                uiArray[i] = parents[i].GetComponentInChildren<GetItemUI>();
            }
        }

        gameObject.SetActive(false);
    }

    public void Play(string itemName, Sprite itemSprite)
    {
        itemIcon.sprite = itemSprite;
        itemInfoText.text = itemName;

        gameObject.SetActive(true);
        tween.DORestartById("In");
    }
}
