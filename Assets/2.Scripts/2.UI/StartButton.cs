using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class StartButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI _text;

    public void OnPointerClick(PointerEventData eventData)
    {
        StadiumManager._instance.ClickforStartOrStop();
    }
    public void isStart()
    {
        _text.text = "STOP\nCOUNT";
    }
    public void isStop()
    {
        _text.text = "GAME\nSTART";
    }
}
