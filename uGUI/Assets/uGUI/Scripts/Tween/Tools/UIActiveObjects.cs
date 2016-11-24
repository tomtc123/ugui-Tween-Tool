using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIActiveObjects : MonoBehaviour
{
    public List<GameObject> active = new List<GameObject>();
    public List<GameObject> deactive = new List<GameObject>();

    Button mButton;

    void Start() {
        mButton = GetComponent<Button>();
        if (mButton != null) {
            mButton.onClick.AddListener(OnClick);
        } else {
            Debug.LogError("找不到 Button.");
        }
    }

    public void OnClick() {
        OnClick(true);
    }

    public void OnClick(bool state) {
        for (int i = 0; i < active.Count; ++i) {
            Set(active[i], state);
        }
        for (int i = 0; i < deactive.Count; ++i) {
            Set(deactive[i], !state);
        }
    }

    void Set(GameObject go, bool state) {
        if (go != null) {
            go.SetActive(state);
        }
    }

}
