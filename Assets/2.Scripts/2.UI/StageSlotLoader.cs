using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
using UnityEngine.EventSystems;

/// <summary>
/// Stage Select�� �ϱ����� ��ư�� ����� ���� ������ ������ִ� Ŭ����
/// Stage�� ���� ���� ��ư�� ������ ��ȹ�̶�� �� Ŭ������ �������
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
