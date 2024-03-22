using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class LaserPointer : MonoBehaviour
{
    // 참조 변수
    [Header("Resources")]
    [SerializeField] GameObject _laser;
    Animator _animator;

    float _settingSpeed = 3;

    // 정보 변수
    bool _isUpDown;
    Vector3 _dir;
    float _nowSpeed;
    bool _move;
    float _mytime;

    int _shootTime;
    int _reTime;
    bool _shoot;

    public void Initialize(bool Upgrade)
    {
        // 참조
        _animator = GetComponent<Animator>();

        // 초기 설정
        _isUpDown = (gameObject.transform.position.y <= -4.5f || gameObject.transform.position.y >= 4.5f) ? false : true;
        _dir = _isUpDown ? new Vector3(0, 1, 0) : new Vector3(1, 0, 0);
        _laser.SetActive(false);
        _move = true;
        _mytime = 0;

        if (Upgrade)
        {
            _shootTime = 4;
            _reTime = 5;
            _nowSpeed = _settingSpeed + _settingSpeed;
        }
        else
        {
            _shootTime = 8;
            _reTime = 10;
            _nowSpeed = _settingSpeed;
        }

    }
    public void LaserActive()
    {
        _mytime += Time.deltaTime;
        if (_move)
        {
            Move();
        }


        if (_mytime >= _reTime)
        {
            _mytime = 0;
            _shoot = false;
        }
        else if (_mytime >= _shootTime)
        {
            if (!_shoot)
            {
                _move = false;
                Shoot();
            }
        }
    }
    public void UpgradeActive()
    {
        _shootTime = 3;
        _reTime = 5;
        _nowSpeed = _settingSpeed  + _settingSpeed;
    }
    // Laser 이동 관련 =========================
    /// <summary>
    /// 이동
    /// </summary>
    public void Move()
    {
        if (_isUpDown)
        {
            if (_laser.transform.position.y >= 4)
            {
                _dir = new Vector3(0, -1, 0);
            }
            else if (_laser.transform.position.y <= -4)
            {
                _dir = new Vector3(0, 1, 0);
            }
        }
        else
        {
            if (_laser.transform.position.x >= 4)
            {
                _dir = new Vector3(-1, 0, 0);
            }
            else if (_laser.transform.position.x <= -4)
            {
                _dir = new Vector3(1, 0, 0);
            }
        }
        gameObject.transform.position += _dir * _nowSpeed * Time.deltaTime;
    }
    //====================================

    // Laser Shoot 관련===================
    /// <summary>
    /// Lazer를 쏘는 신호를 전달
    /// </summary>
    public void Shoot()
    {
        _shoot = true;
        _animator.SetTrigger("Shoot");
    }
    /// <summary>
    /// Lazer를 발사할 때 애니메이션 트리거에서 실행
    /// </summary>
    public void ShootOn()
    {
        SoundManager._instance.PlayEffect(eEffectSound.LaserShoot);
        _laser.SetActive(true);
    }
    /// <summary>
    /// Lazer 발사가 끝날 때 애니메이션 트리거에서 실행
    /// </summary>
    public void ShootOff()
    {
        _laser.SetActive(false);
        _move = true;
    }
    //====================================
}
