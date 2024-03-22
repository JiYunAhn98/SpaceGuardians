using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
using TMPro;
public class SpaceShipGameBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _title;
    [SerializeField] TextMeshProUGUI[] _stageText;

    public void SelectGame()
    {
        _title.text = SceneControlManager._instance._currentScene.ToString();
        for (int i = 1; i <= _stageText.Length; i++)
        {
            _stageText[i-1].text = i + ". " + TableManager._instance.TakeString(TableManager.eTableJsonNames.StageInfo, ((int)SceneControlManager._instance._currentScene), "Explain" + i.ToString());
            Debug.Log(_stageText[i - 1].text);
        }
    }
}
