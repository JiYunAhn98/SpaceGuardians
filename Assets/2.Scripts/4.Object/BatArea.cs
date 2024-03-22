using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatArea : MonoBehaviour
{
    [SerializeField] public ObstacleBat _bat;

    float _time;
    float _moveTime;
    Vector3 _moveVec;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _bat.SetFlying(true);
            _time = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _time += Time.deltaTime;

            if (_time >= 6)
            {
                _time = 0;
                _moveTime = Random.Range(3, 6);
                _moveVec = Vector3.right * Random.Range(3, 6);

            }
            else if (_time <= _moveTime)
            {
                _bat.MovePosition(_moveVec);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _bat.SetFlying(false);
        }
    }
}
