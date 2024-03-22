using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Chemi : Bullet
{
    float _genTime;

    void Update()
    {
        if (_genTime >= 0.1f)
        {
            gameObject.SetActive(false);
        }
        else
        {
            _genTime += Time.deltaTime;
        }
    }
    public override void SpawnInit(float speed, Vector2 dir, float damage)
    {
        SoundManager._instance.PlayEffect(DefineHelper.eEffectSound.ChemiShoot);
        RaycastHit2D rHit;
        LayerMask mask  = LayerMask.GetMask("Block");
        rHit = Physics2D.Raycast(transform.position, dir, 20, mask);
        _dir = dir;
        _damage = damage;
        _genTime = 0;

        transform.localEulerAngles = Vector3.forward * (Quaternion.FromToRotation(Vector3.right, dir).eulerAngles.z);
        transform.localScale = new Vector3(Mathf.Sqrt(Mathf.Pow(rHit.point.x - transform.position.x, 2) + Mathf.Pow(rHit.point.y - transform.position.y, 2)), 1, 0);
    }
    public override float Hit()
    {
        SoundManager._instance.PlayEffect(DefineHelper.eEffectSound.ChemiBoom);
        return _damage;
    }
}
