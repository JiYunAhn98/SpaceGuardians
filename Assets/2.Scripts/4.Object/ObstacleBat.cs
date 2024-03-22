using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBat : MonoBehaviour
{
    [SerializeField] int _batSpeed;
    [SerializeField] Animator _animator;

    public void SetFlying(bool isFlying)
    {
        _animator.SetBool("isFlying", isFlying);
    }

    public void MovePosition(Vector3 pos)
    {
        if(_animator.GetBool("isFlying"))
            transform.position += (transform.position + pos).normalized * _batSpeed * Time.deltaTime;
    }
}
