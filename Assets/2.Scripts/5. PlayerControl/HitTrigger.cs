using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTrigger : MonoBehaviour
{
    Player _player;

    public void HitInit(GameObject player)
    {
        _player = transform.parent.GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 내가 trigger라면 collision이 trigger든 collider든 모두 흡수
        // 내가 collider라면 trigger만 흡수
        if (collision.gameObject.CompareTag("Obstacle") && !_player._isHit)
        {
            _player.Hit(0);
        }
    }
}
