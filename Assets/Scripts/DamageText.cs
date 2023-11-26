using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TextMeshPro text { get; private set; }
    private RectTransform rect;

    void Awake()
    {
        text = GetComponent<TextMeshPro>();
        rect = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        StartEffect();
    }

    public void SetText(int value)
    {
        text.text = value.ToString();
    }

    private void StartEffect()
    {
        Vector2 dirVec = GetRandomVector(-0.5f, 0.5f, 0f, 1f);

        Sequence moveSeq = DOTween.Sequence();
        moveSeq.Append(rect.DOMove(dirVec, 0.75f)
            .SetEase(Ease.OutQuad)
            .SetRelative(true));
        moveSeq.Join(rect.DOScale(1.2f, 0.75f)
            .SetEase(Ease.OutQuad));

        moveSeq.Append(rect.DOMove(Vector3.down, 2)
            .SetEase(Ease.OutCubic)
            .SetRelative(true));
        moveSeq.Join(rect.DOScale(0.75f, 1f)
            .SetEase(Ease.OutCubic));
        moveSeq.Join(text.DOColor(new Color(1, 1, 1, 0), 1f));

        moveSeq.OnComplete(Reload);
    }

    private void Reload()
    {
        rect.DOComplete();
        text.DOComplete();
        text.color = Color.white;
        rect.localScale = Vector3.one;
        gameObject.SetActive(false);
    }

    private Vector2 GetRandomVector(float min, float max)
    {
        return new Vector2(Random.Range(min, max), Random.Range(min, max));
    }

    private Vector2 GetRandomVector(float minX, float maxX, float minY, float maxY)
    {
        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }
}
