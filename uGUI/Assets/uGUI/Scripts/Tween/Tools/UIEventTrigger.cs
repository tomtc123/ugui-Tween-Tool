using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class UIEventTrigger :
    MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerClickHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IDropHandler
{

    public delegate void VoidDelegate(GameObject go);
    public delegate void EventDataDelegate(PointerEventData eventData);

    public EventDataDelegate onDown;
    public EventDataDelegate onUp;
    public EventDataDelegate onClick;
    public EventDataDelegate onBeginDrag;
    public EventDataDelegate onDrag;
    public EventDataDelegate onEndDrag;
    public EventDataDelegate onDrop;

    public static UIEventTrigger Get(GameObject go)
    {
        UIEventTrigger trigger = go.GetComponent<UIEventTrigger>();
        if (trigger == null)
        {
            trigger = go.AddComponent<UIEventTrigger>();
        }
        return trigger;
    }

    public void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (onDown != null)
        {
            onDown(eventData);
        }
    }

    public void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (onUp != null)
        {
            onUp(eventData);
        }
    }

    public void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (onClick != null)
        {
            onClick(eventData);
        }
    }

    public void OnBeginDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (onBeginDrag != null)
        {
            onBeginDrag(eventData);
        }
    }

    public void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (onDrag != null)
        {
            onDrag(eventData);
        }
    }

    public void OnEndDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (onEndDrag != null)
        {
            onEndDrag(eventData);
        }
    }

    public void OnDrop(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (onDrop != null)
        {
            onDrop(eventData);
        }
    }

}
