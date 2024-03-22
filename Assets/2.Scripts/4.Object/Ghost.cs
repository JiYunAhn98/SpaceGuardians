using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy
{
    Animator _animator;

    int _devideNumber;
    float _speed;
    bool _isDevide;
    bool _isDead;

    public bool _divideTirgger { get { return _isDevide; } set { _isDevide = value; } }
    public bool _deadTirgger { get { return _isDead; } }
    public float _speedVal { get { return _speed; } }
    public int _devideNumberVal { get { return _devideNumber; } }

    public void Init(float speed)
    {
        _speed = speed;
        _animator = GetComponent<Animator>();
        _dir = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), 0).normalized;
    }

    public void Move()
    {
        transform.Translate(_dir * _speed * Time.deltaTime);
    }
    public void Hit()
    {
        _dir = Vector3.zero;
        _animator.SetBool("isDead", true);
    }


    #region[애니메이션 함수]
    void Dead()
    {
        gameObject.SetActive(false);
    }
    #endregion[애니메이션 함수]

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Block"))
    //    {
    //        _dir = (gameObject.transform.position - (Vector3)collision.GetContact(0).point).normalized;
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Hit();
        }
    }
}
