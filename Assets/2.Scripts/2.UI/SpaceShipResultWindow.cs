using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
using TMPro;
public class SpaceShipResultWindow : Popup
{
    [SerializeField] TextMeshProUGUI _timeText;
    [SerializeField] GameObject slot;

    StageSlotLoader _stageLoader;
    List<StageSlot> _stageSlots;

    public override void InitPopup()
    {
        if (_stageSlots == null)
        {
            _stageSlots = new List<StageSlot>();
            _stageLoader = GetComponentInChildren<StageSlotLoader>();
            _stageSlots = _stageLoader.LoadSlots(slot);
        }
        ResultPrint();
    }
    public void ResultPrint()
    {
        GameObject manager = GameObject.FindWithTag("Manager");
        _timeText.text = manager.GetComponent<StageBaseManager>()._nowtime;
    }
    public void ReplayBtn()
    {
        SceneControlManager._instance.LoadScene(SceneControlManager._instance._currentScene);
        transform.parent.gameObject.SetActive(false);
    }
    public void ExitGameBtn()
    {
        SceneControlManager._instance.LoadScene(SceneControlManager._instance._returnScene);
        transform.parent.gameObject.SetActive(false);
    }

}
