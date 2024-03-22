using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
using UnityEngine.EventSystems;

/// <summary>
/// Stage Select를 하기위한 버튼을 만들어 내는 구역을 담당해주는 클래스
/// Stage를 고르기 위해 버튼을 생성할 계획이라면 이 클래스를 사용하자
/// </summary>
public class StageSlotLoader : MonoBehaviour
{
    [SerializeField] RectTransform _scrollContents;

    public List<StageSlot> LoadSlots(GameObject _slot)
    {
        List<StageSlot> stageSlots;
        stageSlots = new List<StageSlot>();
        _scrollContents.sizeDelta = new Vector2(0, (int)eSceneName.Count * _slot.GetComponent<RectTransform>().rect.height);

        for (int i = 0; i < (int)eSceneName.Count; i++)
        {
            GameObject go = Instantiate(_slot, _scrollContents);
            go.GetComponent<StageSlot>().InitSetting(((eSceneName)i).ToString());
        }
        return stageSlots;
    }
}
