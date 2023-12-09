using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScriptData
{
    public byte spriteIndex;
    public SpeakerData speakerData;
    [TextArea]
    public string script;
}

[Serializable]
public class ScriptDataSession
{
    public ScriptData[] scriptData;
}

[CreateAssetMenu(fileName = "NewTalkData", menuName = "Scriptable Object/Talk Data")]
public class TalkData : ScriptableObject
{
    public ScriptDataSession[] scriptSession;

    public string GetTalk(byte sessionId, byte scriptIndex)
    {
        return scriptSession[sessionId].scriptData[scriptIndex].script;
    }

    public ScriptData GetScriptData(byte sessionId, byte scriptIndex)
    {
        return scriptSession[sessionId].scriptData[scriptIndex];
    }
}
