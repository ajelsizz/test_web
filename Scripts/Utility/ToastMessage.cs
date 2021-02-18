using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastMessage : MonoBehaviour
{
    [SerializeField] private Image m_Background;
    [SerializeField] private Text m_Text;
    [SerializeField] private float m_StartY = 0f;
    [SerializeField] private float m_Duration = 0.3f;
    [SerializeField] private float m_KeepTime = 3f;
    [SerializeField] private float m_Speed = 500f;
    [SerializeField] private AnimationCurve m_Curve;
    [SerializeField] private bool m_IgnoreTimeScale = true;
    [SerializeField] private bool play = false;

    private bool m_Play;
    private float m_ElapsedTime = 0f;
    private RectTransform rectTransform;

    private string TEST_MSG = "test Message";

    private void Awake()
    {
        rectTransform = m_Background.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (play)
        {
            play = false;
            MakeText(TEST_MSG);
        }

        if (!m_Play)
            return;

        float dt = m_IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
        m_ElapsedTime += dt;
        var percentage = Mathf.Clamp01(m_ElapsedTime / m_Duration);
        var scale = m_Curve.Evaluate(percentage);
        float dy = m_Speed * scale * dt;
        UGUITool.SetAnchoredPositionOffsetY(rectTransform, dy);

        if (percentage >= 1)
        {
            m_Play = false;
            StartCoroutine(DelayHide());
        }
    }

    IEnumerator DelayHide()
    {
        yield return new WaitForSeconds(m_KeepTime);
        m_Background.gameObject.SetActive(false);
    }

    // 飘字提示
    public void MakeText(string msg)
    {
        if (m_Text)
            m_Text.text = msg;
        m_ElapsedTime = 0;
        UGUITool.SetAnchoredPositionY(rectTransform, m_StartY);
        m_Play = true;
        m_Background.gameObject.SetActive(true);
    }
}