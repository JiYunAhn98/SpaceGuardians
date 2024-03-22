using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RollButton : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Player go = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        go._isRoll = true;
    }
}
