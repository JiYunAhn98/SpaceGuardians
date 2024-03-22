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
        // ���� trigger��� collision�� trigger�� collider�� ��� ���
        // ���� collider��� trigger�� ���
        if (collision.gameObject.CompareTag("Obstacle") && !_player._isHit)
        {
            _player.Hit(0);
        }
    }
}
