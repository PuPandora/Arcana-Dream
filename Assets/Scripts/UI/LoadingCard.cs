using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCard : MonoBehaviour
{
    [SerializeField] Image cardImage;
    [SerializeField] private Sprite frontCard;
    [SerializeField] private Sprite backCard;
    public bool isFront = true;
    public bool isBack;

    void Update()
    {
        var y = transform.rotation.eulerAngles.y;

        if (isFront && y > 88f && y < 92f)
        {
            isBack = true;
            isFront = false;
            ChangeCard();
        }
        else if (isBack && y > 268f && y < 272f)
        {
            isFront = true;
            isBack = false;
            ChangeCard();
        }

    }

    public void ChangeCard()
    {
        if (cardImage.sprite == frontCard)
        {
            cardImage.sprite = backCard;
        }
        else
        {
            cardImage.sprite = frontCard;
        }
    }
}
