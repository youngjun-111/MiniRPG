using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnDragHandler = null;


    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        if (OnDragHandler != null)
        {
            OnDragHandler.Invoke(eventData);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointer");
        if (OnClickHandler != null)
        {
            OnClickHandler.Invoke(eventData);
        }
    }
}

