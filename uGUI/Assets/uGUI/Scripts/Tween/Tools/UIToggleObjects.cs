using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Toggle))]
public class UIToggleObjects : MonoBehaviour
{

    public List<GameObject> active = new List<GameObject>();
    public List<GameObject> deactive = new List<GameObject>();

    Toggle toggle;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(DoToggle);
            DoToggle(toggle.isOn);
        }
        else
        {
            Debug.LogError("找不到 Toggle.");
        }
    }

    public void DoToggle()
    {
        DoToggle(toggle.isOn);
    }

    public void DoToggle(bool state)
    {
        for (int i = 0; i < active.Count; ++i)
        {
            Set(active[i], state);
        }
        for (int i = 0; i < deactive.Count; ++i)
        {
            Set(deactive[i], !state);
        }
    }

    void Set(GameObject go, bool state)
    {
        if (go != null)
        {
            go.SetActive(state);
        }
    }

}
