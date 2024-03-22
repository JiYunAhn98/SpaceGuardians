using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair_Jump : Block
{
    public override void Init()
    {
        _collider = gameObject.GetComponent<BoxCollider2D>();
        _collider.isTrigger = true;
    }

    public override void WhenInArea()
    {
        _collider.isTrigger = false;

        PlayerActive.PlayerJump._instance._wasJump = false;
        PlayerActive.PlayerDoubleJump._instance._wasDoubleJump = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().ChangeState(PlayerActive.PlayerIdle._instance);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>()._isJump = true;
    }

    public override void WhenOutArea()
    {
        _collider.isTrigger = true;
    }
}

