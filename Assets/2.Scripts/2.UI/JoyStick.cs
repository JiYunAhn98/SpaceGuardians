using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class JoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerExitHandler
{
    [SerializeField] RectTransform _stickPosition;

    // vector를 가지고 있으면 넘겨주는 함수를 만든다. 해당 방향에 따라서 joystick 영향의 객체가 방향을 가진다.
    RectTransform _rect;
    Vector2 _touch;
    Vector2 _anchor;
    Player _player;

    float _widthHalf;
    bool _sightMode;
    bool _isStop;

    public bool _stopTrigger { get { return _isStop; } set { _isStop = value; } }

    public Vector2 _dirTouch { get { return _touch; } }

    public void PCMovementUpdate()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            _player._moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_sightMode)
        {
            _stickPosition.anchoredPosition = Vector3.zero;
        }
        else
        {
            _player._moveDir = _stickPosition.anchoredPosition = Vector2.zero;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_sightMode)
        {
            _stickPosition.anchoredPosition = Vector2.zero;
        }
        else
        {
            _player._moveDir = _stickPosition.anchoredPosition = Vector2.zero;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_isStop)
        {
            _touch = Vector2.zero;
        }
        else
        {
            _touch = (eventData.position - _anchor) / _widthHalf;
        }

        if (_player._moveSideMode)
        {
            _touch = _touch.x > 0 ? Vector3.right : Vector3.left;
            _stickPosition.anchoredPosition = _touch * _widthHalf;
        }
        else
        {
            if (_touch.magnitude > 1)
            {
                _touch = _touch.normalized;
            }

            _stickPosition.anchoredPosition = _touch * _widthHalf;
        }
        if (_sightMode)
        {
            _player._sightDir = _touch;
        }
        else
        {
            _player._moveDir = _touch;
        }
    }
    public void Init()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _isStop = false;
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            gameObject.SetActive(false);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            _sightMode = false;
            _rect = GetComponent<RectTransform>();
            _widthHalf = _rect.sizeDelta.x * 0.5f;
            _anchor = _rect.anchoredPosition + new Vector2(_widthHalf, _widthHalf);
            _touch = Vector2.zero;
        }
    }
    public void SightInit()
    {
        _sightMode = true;
        _isStop = false;

        _rect = GetComponent<RectTransform>();
        _widthHalf = _rect.sizeDelta.x * 0.5f;
        _anchor = new Vector2(Screen.width + _rect.anchoredPosition.x - _widthHalf, _rect.anchoredPosition.y + _widthHalf);
        _touch = Vector2.zero;

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
}
