using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlideButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Player go = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        go._isSlide = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Player go = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        go._isSlide = false;
    }
}
