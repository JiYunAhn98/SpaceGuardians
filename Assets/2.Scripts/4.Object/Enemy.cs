using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected Vector3 _dir;
    public Vector3 _movedir { get { return _dir; } set { _dir = value.normalized; } }
}
