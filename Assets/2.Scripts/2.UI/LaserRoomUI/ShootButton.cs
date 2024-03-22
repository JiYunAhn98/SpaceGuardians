using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootButton : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Player go = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        go.Shoot();
    }
}
