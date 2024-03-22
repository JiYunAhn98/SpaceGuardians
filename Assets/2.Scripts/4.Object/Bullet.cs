using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [HideInInspector] public float _speed;
    [HideInInspector] public Vector2 _dir;
    [HideInInspector] public float _damage;

    public abstract void SpawnInit(float speed, Vector2 dir, float damage);
    public abstract float Hit();
}
