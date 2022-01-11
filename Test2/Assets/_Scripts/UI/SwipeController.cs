using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeController : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public delegate void OnSwipe(DirectionSwipe direction);
    public OnSwipe SwipeCollBack;
    public enum DirectionSwipe
    {
        Up,
        Down,
        Left,
        Right
    }

    public void OnBeginDrag(PointerEventData eventData) { Swipe(eventData); }
    public void OnDrag(PointerEventData eventData)
    {

    }
    private void Swipe(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y))
        {
            if (eventData.delta.x > 0)
                SwipeCollBack.Invoke(DirectionSwipe.Right);
            else
                SwipeCollBack.Invoke(DirectionSwipe.Left);
        }
        else
        {
            if (eventData.delta.y > 0)
                SwipeCollBack.Invoke(DirectionSwipe.Up);
            else
                SwipeCollBack.Invoke(DirectionSwipe.Down);
        }
    }
}
