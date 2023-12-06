using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpeakerData", menuName = "Scriptable Object/Speaker Data")]
public class Speakerdata : ScriptableObject
{
    public string speakerName;
    public string spearkDesc;
    public Sprite portrait;
}
