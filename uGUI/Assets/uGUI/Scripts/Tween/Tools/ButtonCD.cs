using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class ButtonCD : MonoBehaviour
{

    public float CD = 0.1f;
    float mClickTime = 0f;
    public bool isChangeColor = false;
    Button mButton;

    void Awake()
    {
        mButton = GetComponent<Button>();
        mButton.onClick.AddListener(OnPointerClick);
    }

    void OnEnable()
    {
        SetButtonEnable(true);
    }

    public void OnPointerClick()
    {
        if ((Time.realtimeSinceStartup - mClickTime) >= CD)
        {
            mClickTime = Time.realtimeSinceStartup;
            SetButtonEnable(false);
            StartCoroutine(IESetButtonEnable());
        }
    }

    IEnumerator IESetButtonEnable()
    {
        yield return new WaitForSeconds(CD);
        SetButtonEnable(true);
    }

    void SetButtonEnable(bool enable)
    {
        if (mButton != null)
        {
            mButton.enabled = enable;
            if (isChangeColor)
            {
                Image[] images = mButton.GetComponentsInChildren<Image>();
                Color color = enable ? Color.white : new Color(0.78f, 0.78f, 0.78f, 1f);
                foreach (Image image in images)
                {
                    image.color = color;
                }
            }
        }
    }
}

