using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
using UnityEngine.EventSystems;
using TMPro;

public class StageSlot : MonoBehaviour,IPointerClickHandler
{
    string _myStage;

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneControlManager._instance.LoadScene((eSceneName)System.Enum.Parse(typeof(eSceneName), _myStage));
    }
    public void InitSetting(string scene)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = scene;
        _myStage = scene;
    }
}
