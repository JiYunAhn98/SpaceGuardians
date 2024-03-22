using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
using TMPro;

public class StadiumGameBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] _title;

    public void NowGameAlarm()
    {
        for (int i = 0; i < _title.Length; i++)
        {
            _title[i].text = SceneControlManager._instance._pickStages[i].ToString();
            if (_title[i].text == SceneControlManager._instance._currentScene.ToString())
            {
                _title[i].color = Color.black;
            }
            else
            {
                _title[i].color = Color.gray;
            }
        }
    }
}
