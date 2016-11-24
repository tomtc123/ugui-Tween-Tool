using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Toggle))]
public class UIToggleColor : MonoBehaviour
{
    public Graphic target;

    public Color activeColor;
    public Color deactiveColor;

    Toggle toggle;

    // Use this for initialization
    void Start()
    {
        toggle = GetComponent<Toggle>();
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(OnToggle);
            OnToggle(toggle.isOn);
        }
        else
        {
            Debug.LogError("找不到 Toggle.");
        }
    }

    public void OnToggle(bool state)
    {
        if (target != null)
        {
            target.color = state ? activeColor : deactiveColor;
        }
    }
}
