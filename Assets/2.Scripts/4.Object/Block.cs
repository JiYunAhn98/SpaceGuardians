using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    [HideInInspector] public Collider2D _collider;

    /// <summary>
    /// 처음 실행하는 함수
    /// </summary>
    public abstract void Init();
    /// <summary>
    /// 켜질때 실행할 함수
    /// </summary>
    public abstract void WhenInArea();
    /// <summary>
    /// 꺼질대 실행할 함수
    /// </summary>
    public abstract void WhenOutArea();
}
