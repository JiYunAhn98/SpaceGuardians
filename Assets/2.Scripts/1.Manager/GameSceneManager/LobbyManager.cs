using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
using TMPro;

public class LobbyManager : SceneBaseManager
{
    // 참조 변수
    GameObject[] _lobbyObjs;    // Lobby에 존재하는 Objects
    Camera _maincam;

    // 정보 상수
    float _hoverAlpha = 0.4f;   // 커서 hover상태에 대한 color alpha값 변화값
    float _hoverDistance = 4;   // hover상태로 판단하는 마우스와의 최대 거리

    // 정보 변수
    GameObject _curCursorOn;  // 현재 커서가 올라가있는 오브젝트
    bool _sceneMove;          // 지금 씬이 바뀌는 중인지 체크

    public override void UpdateFrame()
    {
        Vector3 point;
        //카메라 마우스 클릭 위치 받아오기
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

        point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));   //마우스 커서 위치 가져오기

#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            point = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, -Camera.main.transform.position.z));
        }
        else 
        {
            point = Vector3.zero;
        }
#endif
        for (int i = 0; i < _lobbyObjs.Length; i++)
        {
            float distance = Vector2.Distance(point, _lobbyObjs[i].transform.position);

            if (_curCursorOn == null && distance <= _hoverDistance)
                ObjAnimCursorOn(_lobbyObjs[i]);
            else if (distance > _hoverDistance && _curCursorOn == _lobbyObjs[i])
                ObjAnimOriginal(_lobbyObjs[i]);
        }
        if (_curCursorOn == null)
            _maincam.transform.position = Vector3.Lerp(_maincam.transform.position, Vector3.back * 10, Time.deltaTime * 5);
        else
            _maincam.transform.position = Vector3.Lerp(_maincam.transform.position, _curCursorOn.transform.position.normalized + Vector3.back * 10, Time.deltaTime * 5);

        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)) ClickObject();
    }

    public override void ProgInit()
    {
        _sceneMove = false;
        _lobbyObjs = GameObject.FindGameObjectsWithTag("LobbyObject");
        for (int i = 0; i < _lobbyObjs.Length; i++)
            ObjAnimOriginal(_lobbyObjs[i]);
        _maincam = Camera.main;
    }
    /// <summary>
    /// _curCursorOn에 현재 올라가있는 Obejct를 저장해두고 Hover에 걸맞는 연출을 플레이
    /// </summary>
    /// <param name="obj"> 현재 커서가 올라가 있는 Object </param>
    public void ObjAnimCursorOn(GameObject obj)
    {
        _curCursorOn = obj;

        SoundManager._instance.PlayEffect(eEffectSound.LobbyHover);
        obj.GetComponentInChildren<SpriteRenderer>().color += new Color(0, 0, 0, _hoverAlpha);
        obj.GetComponentInChildren<TextMeshProUGUI>().color += new Color(0, 0, 0, _hoverAlpha);
        obj.GetComponentInChildren<Animator>().speed *= 4;
    }
    /// <summary>
    /// _curCursorOn를 비우고 원래 연출을 플레이
    /// </summary>
    public void ObjAnimOriginal(GameObject obj)
    {
        obj.GetComponentInChildren<SpriteRenderer>().color -= new Color(0, 0, 0, _hoverAlpha);
        obj.GetComponentInChildren<TextMeshProUGUI>().color -= new Color(0, 0, 0, _hoverAlpha);
        obj.GetComponentInChildren<Animator>().speed /= 4;

        _curCursorOn = null;
    }
    /// <summary>
    /// 마우스를 클릭하는 경우 _curCursorOn의 객체에 해당하는 Scene으로 이동
    /// </summary>
    public void ClickObject()
    {
        if (_curCursorOn == null || _sceneMove) return;
        _sceneMove = true;
        SceneControlManager._instance.LoadScene((eSceneName)System.Enum.Parse(typeof(eSceneName), _curCursorOn.name));
    }

}

