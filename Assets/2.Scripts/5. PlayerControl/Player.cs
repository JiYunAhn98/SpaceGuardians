using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
using PlayerActive;

public class Player : FSM<Player>
{
    // 참조 변수
    SpriteRenderer _model;
    BoxCollider2D _col;
    BoxCollider2D _floorTrigger;
    HitTrigger _hitTrigger;
    Rigidbody2D _rigid;
    Animator _animator;
    Gun _gun;
    GameObject _shadow;

    // 정보 변수
    Vector3 _lastDir;
    Vector3 _curMoveDir;
    Vector3 _curSightDir;
    bool _moveSide;

    // 상태 변수
    float _moveSpeed;
    float _jumpPower;
    float _life;

    // 상황 변수
    bool _death;
    bool _roll;
    bool _jump;
    bool _fall;
    bool _hit;
    bool _slide;
    bool _doubleJump;

    public Rigidbody2D _rigidCtr { get { return _rigid; } }
    public BoxCollider2D _colCtr { get { return _col; } }
    public BoxCollider2D _floorTriggerCtr { get { return _floorTrigger; } }
    public Animator _animatorCtr { get { return _animator; } }
    public float _moveSpeedVal { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public bool _moveSideMode { get { return _moveSide; } }
    public float _lifeVal { get { return _life; } set { _life = value; } }
    public Vector3 _moveDir
    {
        get { return _curMoveDir; }
        set
        {
            if (value != Vector3.zero || _curMoveDir != Vector3.zero)
            {
                _lastDir = _curMoveDir;
            }
            _curMoveDir = value.normalized;
        }
    }
    public Vector3 _lastMovDir { get { return _lastDir; } }
    public Vector3 _sightDir
    {
        get { return _curSightDir; }
        set
        {
            _curSightDir = value.normalized;
        }
    }
    public bool _isDeath { get { return _death; } set { _death = value; } }
    public bool _isRoll { get { return _roll; } set { _roll = value; } }
    public bool _isJump { get { return _jump; } set { _jump = value; } }
    public bool _isFall { get { return _fall; } set { _fall = value; } }
    public bool _isHit { get { return _hit; } set { _hit = value; } }
    public bool _isSlide { get { return _slide; } set { _slide = value; } }
    public bool _isDoubleJump { get { return _doubleJump; } set { if (_jump || _fall) _doubleJump = value; else _doubleJump = false; } }

    void Update()
    {
        FSMUpdate();
    }
    public void Initialize(float life, float moveSpeed, IFSMState<Player> nowstate, float jumpPower = 0, bool moveSide = false, bool shooting = false)
    {
        // 초기 참조
        _model = GetComponentInChildren<SpriteRenderer>();
        _rigid = GetComponentInChildren<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _col = GetComponent<BoxCollider2D>();
        _floorTrigger = transform.GetChild(1).GetComponent<BoxCollider2D>();
        _hitTrigger = transform.GetChild(2).GetComponent<HitTrigger>();
        _shadow = transform.GetChild(3).gameObject;
        _gun = transform.GetComponentInChildren<Gun>();

        // 초기 설정
        _hitTrigger.HitInit(gameObject);
        _lifeVal = life;
        _moveSide = moveSide;
        _moveSpeed = moveSpeed;
        _jumpPower = jumpPower;
        _rigidCtr.mass = 1;

        if (moveSide)
        {
            _floorTrigger.enabled = true;
            _rigidCtr.gravityScale = 1;
            _shadow.SetActive(false);
        }
        else
        {
            _floorTrigger.enabled = false;
            _rigidCtr.gravityScale = 0;
            _shadow.SetActive(true);
        }

        if (shooting)
        {
            _gun.gameObject.SetActive(true);
            _gun.InitGunStat(moveSpeed);
            _curSightDir = Vector3.right;
        }
        else
        {
            _gun.gameObject.SetActive(false);
            _curSightDir = Vector3.zero;
        }

        //  최초 동작 실행
        InitState(this, nowstate);
        _isRoll = false;
        _isDeath = false;
        _isFall = false;
        _isHit = false;
        _isJump = false;
        _slide = false;
        _isDoubleJump = false;
    }

    #region [FSM Active Functions]
    /// <summary>
    /// FSM의 Enter에서 발생하는 이벤트
    /// </summary>
    /// <param name="anim"></param>
    public void ChangeCharacterAnimation(eAnimState anim)
    {
        _animator.SetInteger("AnimState", (int)anim);
        _animator.SetTrigger("ChangeState");
    }
    public void ChangeSortingLayer(string layer)
    {
        GetComponentInChildren<SpriteRenderer>().sortingLayerName = layer;
    }
    public void MoveToDir()
    {
        if (_isDeath) return;

        if (_curMoveDir == Vector3.zero)
        {
            if (_moveSide)
                _rigid.velocity = new Vector2(0, _rigid.velocity.y);
            else
                _rigid.velocity = Vector2.zero;
            return;
        }

        Vector3 originPos = transform.position;
        float _distance = _moveSpeed * Time.deltaTime;

        if (_moveSideMode)
        {
            _rigid.velocity = new Vector2(_moveDir.x * _moveSpeedVal, _rigid.velocity.y);
        }
        else
        {
            _rigid.MovePosition(originPos + _curMoveDir * _distance);
        }

        if (_curSightDir == Vector3.zero)
        {
            _model.flipX = (_moveDir.x > 0) ? false : true;
        }
    }
    public void Roll(Vector3 rollDir)
    {
        if (_isDeath) return;
        //Vector3 rollDir = (_curMoveDir == Vector3.zero) ? _lastDir : _curMoveDir;

        float _goalPos = _moveSpeed * 2 * Time.deltaTime;
        _rigid.MovePosition(transform.position + rollDir * _goalPos);

        _model.flipX = (rollDir.x > 0) ? false : true;
    }
    public void Jump()
    {
        if (_isDeath) return;
        _rigid.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        _rigidCtr.mass = 1;
        _rigidCtr.gravityScale = 1;
        _isJump = true;
    }
    public void Hit(float damage)
    {
        _lifeVal -= damage;
        if(_lifeVal <= 0)
            ChangeState(PlayerDeath._instance);
        else
            ChangeState(PlayerHit._instance);
    }
    public void Shoot()
    {
        transform.localScale = (_sightDir.x >= 0) ? Vector3.up + Vector3.right : Vector3.up + Vector3.left;
        _gun.Shoot();
    }
    #endregion [FSM Active Functions]

    #region [외부 함수]
    /// <summary>
    /// Ray를 쏴서 내가 원하는 것이 있는 지 확인
    /// </summary>
    /// <param name="distance"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public bool HitRayMoveDistanceToLayer(Vector3 distance, string layer)
    {
        int lMask = 1 << LayerMask.NameToLayer(layer);

        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, distance, lMask);

        return (rayHit.transform != null) ? true : false;
    }

    #endregion[외부 함수]

    //private void OnGUI()
    //{
    //    GUIStyle uIStyle = new GUIStyle();
    //    uIStyle.fontSize = 50;
    //    uIStyle.normal.textColor = Color.red;
    //    GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "_curMoveDir : " + _curMoveDir.ToString(), uIStyle);
    //    GUI.Label(new Rect(0, 60, Screen.width, Screen.height), "_moveDir : " + _moveDir.ToString(), uIStyle);
    //    GUI.Label(new Rect(0, 120, Screen.width, Screen.height), "_moveSpeed : " + _moveSpeed.ToString(), uIStyle);
    //}

}
