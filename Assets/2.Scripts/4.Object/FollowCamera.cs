using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Transform _followObject;
    Vector3 _offset;
    bool _fixedAxisX;
    bool _fixedAxisY;

    public Vector3 _offsetSetting { get { return _offset; } set { _offset = value; } }

    public void Follow()
    {
        Vector3 destination = new Vector3();

        if (_followObject != null)
        {
            destination = _followObject.position + _offset;
        }
        if (_fixedAxisX)
        {
            destination.x = transform.position.x;
        }
        if (_fixedAxisY)
        {
            destination.y = transform.position.y;
        }

        transform.position = destination;
    }

    public void SetUp(Transform followObject, Vector3 offset, bool fixX = false, bool fixY = false)
    {
        _followObject = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _followObject = followObject;
        _offset = offset - new Vector3(0, 0, 10);
        _fixedAxisX = fixX;
        _fixedAxisY = fixY;
        
        Follow();
    }
}
