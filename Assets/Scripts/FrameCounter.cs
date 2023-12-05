using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameCounter : MonoBehaviour
{
    private float deltaTime;

    [SerializeField]
    private int size = 25;

    [SerializeField]
    private Color color = Color.green;

    public bool isShow;

    void Start()
    {
        Debug.Log("Frame Counter가 있습니다.\n빌드할 떄 잊지말고 제거하세요.");
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        if (Input.GetKeyDown(KeyCode.F1))
        {
            isShow = !isShow;
        }
    }

    void OnGUI()
    {
        if (isShow)
        {
            GUIStyle style = new GUIStyle();
            Rect rect = new Rect(30, 30, Screen.width, Screen.height);

            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = size;
            style.normal.textColor = color;

            float ms = deltaTime / 1000f;
            float fps = 1.0f / deltaTime;
            string text = $"{fps:0} FPS\n{ms:0.00} ms";

            GUI.Label(rect, text, style);
        }
    }
}
