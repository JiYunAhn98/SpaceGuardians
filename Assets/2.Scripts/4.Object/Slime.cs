using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    Animator _animator;

    int _devideNumber;
    float _speed;
    float _maxHp;
    float _nowHp;
    bool _isDevide;
    bool _isDead;

    public bool _divideTirgger { get { return _isDevide; } set { _isDevide = value; } }
    public bool _deadTirgger { get { return _isDead; } }
    public float _speedVal { get { return _speed; } }
    public float _maxHpVal { get { return _maxHp; } }
    public int _devideNumberVal { get { return _devideNumber; } }

    public void Init(float speed, float hp, int devideNum)
    {
        _maxHp = _nowHp = hp;
        _isDead = false;
        _isDevide = false;
        _devideNumber = devideNum;
        _speed = speed;
        _animator = GetComponent<Animator>();
        transform.localScale = new Vector3(2, 2, 0) * devideNum;
        while (_dir.magnitude <= 0.5f)
        {
            _dir = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), 0).normalized;
        }
        
    }
    public void Move()
    {
        transform.Translate(_dir * _speed * Time.deltaTime);
    }
    public void Hit(float damage)
    {
        _nowHp -= damage;

        if (_nowHp <= 0)
        {
            _dir = Vector3.zero;
            _animator.SetTrigger("isDead");
        }
    }

    /// <summary>
    /// 애니메이션용 함수
    /// </summary>
    public void DevideTime()
    {
        _devideNumber--;

        if (_devideNumber <= 0)
        {
            _isDead = true;
            gameObject.SetActive(false);
            return;
        }

        transform.localScale -= new Vector3(2, 2, 0);
        _speed += 0.5f;

        _nowHp = _maxHp;
        while (_dir.magnitude == 0)
        {
            _dir = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), 0).normalized;
        }
        _isDevide = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            Hit(bullet.Hit());
        }
    }
}
