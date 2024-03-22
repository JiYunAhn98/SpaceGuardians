using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossProductCollider : MonoBehaviour
{
    [SerializeField] Vector3 _normalVec;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy._movedir = new Vector3(enemy._movedir.x + _normalVec.x * Mathf.Abs(enemy._movedir.x) * 2, enemy._movedir.y + _normalVec.y * Mathf.Abs(enemy._movedir.y) * 2, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            collision.gameObject.GetComponent<Bullet>().Hit();
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy._movedir = new Vector3(enemy._movedir.x + _normalVec.x * Mathf.Abs(enemy._movedir.x) * 2, enemy._movedir.y + _normalVec.y * Mathf.Abs(enemy._movedir.y) * 2, 0);
        }
    }
}
