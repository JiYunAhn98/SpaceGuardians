using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
using TMPro;

public class LobbyManager : SceneBaseManager
{
    // ���� ����
    GameObject[] _lobbyObjs;    // Lobby�� �����ϴ� Objects
    Camera _maincam;

    // ���� ���
    float _hoverAlpha = 0.4f;   // Ŀ�� hover���¿� ���� color alpha�� ��ȭ��
    float _hoverDistance = 4;   // hover���·� �Ǵ��ϴ� ���콺���� �ִ� �Ÿ�

    // ���� ����
    GameObject _curCursorOn;  // ���� Ŀ���� �ö��ִ� ������Ʈ
    bool _sceneMove;          // ���� ���� �ٲ�� ������ üũ

    public override void UpdateFrame()
    {
        Vector3 point;
        //ī�޶� ���콺 Ŭ�� ��ġ �޾ƿ���
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

        point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));   //���콺 Ŀ�� ��ġ ��������

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
    /// _curCursorOn�� ���� �ö��ִ� Obejct�� �����صΰ� Hover�� �ɸ´� ������ �÷���
    /// </summary>
    /// <param name="obj"> ���� Ŀ���� �ö� �ִ� Object </param>
    public void ObjAnimCursorOn(GameObject obj)
    {
        _curCursorOn = obj;

        SoundManager._instance.PlayEffect(eEffectSound.LobbyHover);
        obj.GetComponentInChildren<SpriteRenderer>().color += new Color(0, 0, 0, _hoverAlpha);
        obj.GetComponentInChildren<TextMeshProUGUI>().color += new Color(0, 0, 0, _hoverAlpha);
        obj.GetComponentInChildren<Animator>().speed *= 4;
    }
    /// <summary>
    /// _curCursorOn�� ���� ���� ������ �÷���
    /// </summary>
    public void ObjAnimOriginal(GameObject obj)
    {
        obj.GetComponentInChildren<SpriteRenderer>().color -= new Color(0, 0, 0, _hoverAlpha);
        obj.GetComponentInChildren<TextMeshProUGUI>().color -= new Color(0, 0, 0, _hoverAlpha);
        obj.GetComponentInChildren<Animator>().speed /= 4;

        _curCursorOn = null;
    }
    /// <summary>
    /// ���콺�� Ŭ���ϴ� ��� _curCursorOn�� ��ü�� �ش��ϴ� Scene���� �̵�
    /// </summary>
    public void ClickObject()
    {
        if (_curCursorOn == null || _sceneMove) return;
        _sceneMove = true;
        SceneControlManager._instance.LoadScene((eSceneName)System.Enum.Parse(typeof(eSceneName), _curCursorOn.name));
    }

}

