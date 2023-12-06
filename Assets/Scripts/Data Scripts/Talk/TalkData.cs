using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScriptData
{
    public byte Index;
    public Speakerdata speakerData;
    [TextArea]
    public string script;
}

[CreateAssetMenu(fileName = "NewTalkData", menuName = "Scriptable Object/Talk Data")]
public class TalkData : ScriptableObject
{
    public ScriptData[] talkSession0;
    public ScriptData[] talkSession1;
    public ScriptData[] talkSession2;
    public ScriptData[] talkSession3;
    public ScriptData[] talkSession4;

    public string GetTalk(int scriptId, int scriptIndex)
    {
        ScriptData[] targetScript = null;

        // 스크립트 세션 탐색
        switch (scriptId)
        {
            case 0:
                targetScript = talkSession0;
                break;
            case 1:
                targetScript = talkSession1;
                break;
            case 2:
                targetScript = talkSession2;
                break;
            case 3:
                targetScript = talkSession3;
                break;
            case 4:
                targetScript = talkSession4;
                break;
        }

        return targetScript[scriptIndex].script;
    }
}
