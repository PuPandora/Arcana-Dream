using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    [SerializeField] TalkData tutorialScript;
    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] private bool isPressKey;

    WaitUntil waitUntilPress;

    void Awake()
    {
        #region 싱글톤
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    void Start()
    {
        PrintTutorialText();
    }

    // 검은 화면
    // 텍스트만 출력됨 (Tutorial Talk Data)

    public void PrintTutorialText()
    {
        StartCoroutine(PrintTextRoutine());
    }

    public IEnumerator PrintTextRoutine()
    {
        waitUntilPress = new WaitUntil(() => isPressKey);

        for (byte i = 0; i < tutorialScript.talkSession0.Length; i++)
        {
            tutorialText.text = string.Empty;
            string script = tutorialScript.talkSession0[i].script;

            tutorialText.text = $"\"{script}\"";
            yield return waitUntilPress;
        }
        yield return null;
    }
}
