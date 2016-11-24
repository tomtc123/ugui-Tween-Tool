using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

// 按下间断发送消息
[RequireComponent(typeof(EventTrigger))]
[AddComponentMenu("UI/Plus/PressGapEvent")]
public class PressGapEvent : MonoBehaviour
{
    public UnityAction pressAction;
    public UnityAction releaseAction;

    public bool available;//可用状态

    public float gap = 0.5f;
    public bool startDelay = true;

    float m_lastTime = -999f;

    bool m_pressed = false;

    void Awake()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();

        EventTrigger.Entry press = new EventTrigger.Entry();
        press.eventID = EventTriggerType.PointerDown;
        press.callback.AddListener(this.OnPress);
        trigger.triggers.Add(press);

        EventTrigger.Entry release = new EventTrigger.Entry();
        release.eventID = EventTriggerType.PointerUp;
        release.callback.AddListener(this.OnRelease);
        trigger.triggers.Add(release);

        available = true;
    }

    void OnDisable()
    {
        m_pressed = false;
    }

    void Update()
    {
        if (!available)
            return;

        if (!m_pressed)
            return;

        if (Time.realtimeSinceStartup - m_lastTime < gap)
            return;

        m_lastTime = Time.realtimeSinceStartup;

        if(pressAction != null) pressAction();
    }

    void OnPress(BaseEventData data)
    {
        m_pressed = true;

        if (startDelay)
            m_lastTime = Time.realtimeSinceStartup;
        else
            m_lastTime = -999f;
    }

    void OnRelease(BaseEventData data)
    {
        m_pressed = false;

        if (available && releaseAction != null) releaseAction();
        else available = true;
    }
}