using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeartRecoverSystem : MonoBehaviour
{
    [SerializeField] Slider _timingBar;
    [SerializeField] TextMeshProUGUI _gageCount;
    [SerializeField] RectTransform _successZone;

    [SerializeField] float speedVal;

    Player _player;
    bool _dir;
    bool _isSuccess;
    float _speed;
    int _count;

    public bool UpdateSystem()
    {
        if (_timingBar.value >= _timingBar.maxValue)
        {
            _dir = false;
        }
        else if (_timingBar.value <= _timingBar.minValue)
        {
            _dir = true;
        }

        _timingBar.value += Time.deltaTime * (_dir ? _speed : -_speed);

        return _isSuccess;
    }

    public void InitSystem()
    {
        _timingBar.value = 0;
        _count = 1;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gameObject.SetActive(false);
    }

    public void OpenSystem()
    {
        if (gameObject.activeSelf)
        {
            _count++;
            return;
        }
        _count = 1;
        _speed += speedVal;
        gameObject.SetActive(true);
        _isSuccess = false;
        _gageCount.text = ((int)_player._lifeVal).ToString();

        if(_successZone.sizeDelta.x >= 0.2f)
            _successZone.sizeDelta = new Vector2(_successZone.sizeDelta.x / 2, _successZone.sizeDelta.y);
    }

    public void HeartButtonClick()
    {
        float heartVal = Mathf.Abs(_timingBar.value - _timingBar.maxValue / 2);

        // ÀÌÆåÆ® ³ª¿À°Ô²û ÇÏ±â

        if (heartVal > _successZone.rect.width / 2)
        {
            int damage = _count * (int)(heartVal * 100 / _timingBar.maxValue);

            _player.Hit(damage);
            _gageCount.text = (_player._lifeVal <= 0) ? "0" : _player._lifeVal.ToString();
        }
        else
        {
            _isSuccess = true;
            gameObject.SetActive(false);
            _player.ChangeState(PlayerActive.PlayerIdle._instance);
        }
    }
}
