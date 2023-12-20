using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCharacter : MonoBehaviour
{
    Image image;
    public Sprite[] sprites;
    private enum SpriteType { Random, Walk, Run, Win }
    [SerializeField] private SpriteType type;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        switch (type)
        {
            case SpriteType.Random:
                var sprites = LoadingManager.instance.sprites;
                int randNum = Random.Range(0, sprites.Count);
                StartCoroutine(
                    PlayAnimationRoutine(sprites[randNum])
                    );
                break;
            case SpriteType.Walk:
                StartCoroutine(PlayAnimationRoutine(LoadingManager.instance.walkSprites));
                break;
            case SpriteType.Run:
                StartCoroutine(PlayAnimationRoutine(LoadingManager.instance.runSprites));
                break;
            case SpriteType.Win:
                StartCoroutine(PlayAnimationRoutine(LoadingManager.instance.winSprites));
                break;
        }
    }

    private IEnumerator PlayAnimationRoutine(Sprite[] sprites)
    {
        int index = 0;
        while (true)
        {
            image.sprite = sprites[index++ % sprites.Length];
            yield return Utils.delay0_1;
        }
    }
}
