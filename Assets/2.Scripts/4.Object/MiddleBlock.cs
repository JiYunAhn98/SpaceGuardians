using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleBlock : Block
{
    public override void Init()
    {
        _collider = gameObject.GetComponent<CompositeCollider2D>();
        _collider.isTrigger = true;
    }

    public override void WhenInArea()
    {
        _collider.isTrigger = false;
    }

    public override void WhenOutArea()
    {
        _collider.isTrigger = true;
    }
}
