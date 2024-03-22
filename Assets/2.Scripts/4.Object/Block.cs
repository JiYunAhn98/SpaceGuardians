using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    [HideInInspector] public Collider2D _collider;

    /// <summary>
    /// ó�� �����ϴ� �Լ�
    /// </summary>
    public abstract void Init();
    /// <summary>
    /// ������ ������ �Լ�
    /// </summary>
    public abstract void WhenInArea();
    /// <summary>
    /// ������ ������ �Լ�
    /// </summary>
    public abstract void WhenOutArea();
}
