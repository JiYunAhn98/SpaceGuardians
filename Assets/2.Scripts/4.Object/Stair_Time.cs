using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair_Time : Block
{
    [SerializeField] GameObject _onStair;
    [SerializeField] GameObject _offStair;

    bool _startGimic;
    bool _readyGimic;

    public override void Init()
    {
        _collider = gameObject.GetComponent<BoxCollider2D>();
        _collider.isTrigger = true;
        _startGimic = false;
        _readyGimic = false;
    }

    public override void WhenInArea()
    {
        if (!_startGimic)
        {
            _collider.isTrigger = false;
            if (!_readyGimic)
            {
                StartCoroutine(OnMyEvent());
            }
        }
        else
        {
            _collider.isTrigger = true;
        }
    }

    public override void WhenOutArea()
    {
        _collider.isTrigger = true;
    }

    public IEnumerator OnMyEvent()
    {
        _readyGimic = true;
        yield return new WaitForSeconds(3);

        _startGimic = true;
        _collider.isTrigger = true;
        _onStair.SetActive(false);
        _offStair.SetActive(true);

        yield return new WaitForSeconds(2);
        _onStair.SetActive(true);
        _offStair.SetActive(false);
        _startGimic = false;
        _readyGimic = false;

        yield break;
    }
}
