using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpeakerData", menuName = "Scriptable Object/Speaker Data")]
public class SpeakerData : ScriptableObject
{
    public string speakerName;
    public string spearkDesc;
    [PreviewField(50, ObjectFieldAlignment.Left)]
    public Sprite[] portraits;

    [Title("Color for Portrait Shader")]
    [ColorUsage(true, true)]
    public Color baseColor;
    [ColorUsage(true, true)]
    public Color eyeColor;
    public float dark;

    [Title("SFX")]
    public AudioClip talkSfx;
    [Range(0f, 2f)]
    public float volumeIntensity = 1f;
    public Vector2 pitch = Vector2.one;
}
