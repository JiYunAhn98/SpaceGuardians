using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Burn : Bullet
{
    GameObject _boomShoot;
    CircleCollider2D _trigger;

    void Update()
    {
        transform.Translate(_dir * _speed * Time.deltaTime);
    }

    public override void SpawnInit(float speed, Vector2 dir, float damage)
    {
        SoundManager._instance.PlayEffect(DefineHelper.eEffectSound.BurnShoot);
        _speed = speed;
        _dir = Vector3.right;
        _damage = damage;
        transform.localEulerAngles = Vector3.forward * (Quaternion.FromToRotation(Vector3.right, dir).eulerAngles.z);
        _boomShoot = gameObject.transform.GetChild(1).gameObject;
        _boomShoot.SetActive(false);
        _trigger = GetComponent<CircleCollider2D>();
    }
    public override float Hit()
    {
        if (!_boomShoot.activeSelf)
        {
            StartCoroutine(Boom());
        }
        return _damage;
    }
    public IEnumerator Boom()
    {
        SoundManager._instance.PlayEffect(DefineHelper.eEffectSound.BurnBoom);
        _dir = Vector2.zero;
        _boomShoot.SetActive(true);
        _trigger.radius = 0.2f;
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
