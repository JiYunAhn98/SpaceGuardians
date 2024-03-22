using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair_Move : Block
{
    bool _isMoveLeft;
    public override void Init()
    {
        _collider = gameObject.GetComponent<BoxCollider2D>();
        _collider.isTrigger = true;
        StartCoroutine(OnMyEvent());
    }

    public override void WhenInArea()
    {
        _collider.isTrigger = false;
    }

    public override void WhenOutArea()
    {
        _collider.isTrigger = true;
    }

    public IEnumerator OnMyEvent()
    {
        while (true)
        {
            if (_isMoveLeft)
            {
                transform.position += Vector3.left * Time.deltaTime;
                if (transform.position.x < -5) _isMoveLeft = !_isMoveLeft;
            }
            else
            {
                transform.position += Vector3.right * Time.deltaTime;
                if (transform.position.x > 5) _isMoveLeft = !_isMoveLeft;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
