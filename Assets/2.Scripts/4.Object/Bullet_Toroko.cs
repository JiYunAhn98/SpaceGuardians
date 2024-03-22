using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Toroko : Bullet
{
    void Update()
    {
        transform.Translate(_dir * _speed * Time.deltaTime);
    }

    public override void SpawnInit(float speed, Vector2 dir, float damage)
    {
        SoundManager._instance.PlayEffect(DefineHelper.eEffectSound.TorokoShoot);
        _speed = speed;
        _dir = Vector3.right;
        _damage = damage;
        transform.localEulerAngles = Vector3.forward * (Quaternion.FromToRotation(Vector3.right, dir).eulerAngles.z);
    }
    public override float Hit()
    {
        gameObject.SetActive(false);
        SoundManager._instance.PlayEffect(DefineHelper.eEffectSound.TorokoBoom);

        return _damage;
    }
}
