using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMoveAttacker : MonoBehaviour
{
    [SerializeField] Transform _startPos;
    [SerializeField] Transform _finishPos;
    [SerializeField] Transform _attacker;

    int _moveDir;
    int _speed;

    void Awake()
    {
        _moveDir = 1;
        _speed = Random.Range(3,11);
    }

    void Update()
    {
        _attacker.Rotate(Vector3.forward * 10);

        if (_attacker.position.x <= _startPos.position.x)
        {
            _moveDir = 1;
        }
        else if (_attacker.position.x >= _finishPos.position.x)
        {
            _moveDir = -1;
        }

        _attacker.position += Vector3.right * _moveDir * _speed * Time.deltaTime;
    }

}
