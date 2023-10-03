using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ButtonEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static event EventHandler<GameObject> onClick;
    public static event EventHandler<GameObject> onRelease;

    public void OnPointerUp(PointerEventData eventData)
    {
        onClick?.Invoke(this, this.gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onRelease?.Invoke(this, this.gameObject);
    }
}
