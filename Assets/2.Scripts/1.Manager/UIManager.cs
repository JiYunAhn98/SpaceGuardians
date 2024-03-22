using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class UIManager : MonoSingleton<UIManager>
{
    GameObject[] _prefabPopups;

    Dictionary<ePopup, GameObject> _popups;

    void Awake()
    {
        _popups = new Dictionary<ePopup, GameObject>();
        _prefabPopups = new GameObject[(int)ePopup.Count];
        for (int i=0; i< (int)ePopup.Count; i++)
        {
            _prefabPopups[i] = Resources.Load("UI/Popup/" + ((ePopup)i).ToString()) as GameObject;
        }
    }
    public void OpenPopup(ePopup popupName)
    {
        if (_popups.ContainsKey(popupName))
        {
            _popups[popupName].SetActive(true);
        }
        else
        {
            _popups.Add(popupName, Instantiate(_prefabPopups[(int)popupName], transform));
        }
        _popups[popupName].GetComponentInChildren<Popup>().InitPopup();
    }
    public void ClosePopup(ePopup popupName)
    {
        if (_popups.ContainsKey(popupName))
        {
            _popups[popupName].SetActive(false);
        }
        else
        {
            Debug.LogError("팝업이 존재하지 않습니다.");
            return;
        }
    }
    public void CloseAllPopup()
    {
        foreach (ePopup popupName in _popups.Keys)
        {
            _popups[popupName].SetActive(false);
        }
    }
}
