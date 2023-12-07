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
}
