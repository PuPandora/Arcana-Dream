using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
    public enum UIType { Button }
    public UIType type;
    public AudioManager.Sfx sfxType;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        AudioManager.instance.PlaySfx(sfxType, 1.2f, 1.5f);
    }
}
