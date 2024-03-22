using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimerBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _txtSec;
    [SerializeField] TextMeshProUGUI _txtMin;
    public void OpenTimerBox(float startTime)
    {
        gameObject.SetActive(true);
        SetTime(startTime);
    }
    public void CloseTimerBox()
    {
        gameObject.SetActive(false);
        SetTime(0);
    }
    public void SetTime(float nowTime)
    {
        int min = (int)nowTime / 60;
        int sec = (int)nowTime % 60;

        _txtMin.text = (min >= 10) ? min.ToString() : "0" + min.ToString();
        _txtSec.text = (sec > 9) ? sec.ToString() : "0" + sec.ToString();
    }
    public string GetTime()
    {
        return _txtMin.text + ":" + _txtSec.text;
    }
}
