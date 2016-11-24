using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class SpinWithMouse : MonoBehaviour, IDragHandler
{
    public float speed = 1f;
    public Transform target;

    // Use this for initialization
    void Start()
    {
        if (!target)
            target = transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (target)
            target.localRotation = Quaternion.Euler(0f, -0.5f * speed * eventData.delta.x, 0f) * target.localRotation;
    }
}
