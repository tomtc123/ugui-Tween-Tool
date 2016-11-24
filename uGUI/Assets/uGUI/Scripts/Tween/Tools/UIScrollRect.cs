using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class UIScrollRect : ScrollRect
{
    public Action<PointerEventData> onBeginDrag;
    public Action<PointerEventData> onDrag;
    public Action<PointerEventData> onEndDrag;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        if (onBeginDrag != null)
        {
            onBeginDrag(eventData);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        if (onDrag != null)
        {
            onDrag(eventData);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (onEndDrag != null)
        {
            onEndDrag(eventData);
        }
    }

}
