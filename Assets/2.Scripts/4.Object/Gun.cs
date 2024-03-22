using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
public class Gun : MonoBehaviour
{
    GameObject _bulletPrefab;
    Transform _bulletPos;
    Transform _ammo;
    float _bulletSpeed;

    List<Bullet> _instanBullets;
    Player _player;
    float _coolTime;
    float _prevTime;
    float _damage;

    public void InitGunStat(float playerSpeed)
    {
        eCharacterName nowPlayer = PlayerManager._instance._nowPickCharacter;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _bulletPos = transform.GetChild(0).transform;

        _damage = TableManager._instance.Takefloat(TableManager.eTableJsonNames.CharacterInfo, (int)nowPlayer, TableManager.eCharacterInfoIndex.Damage.ToString());
        _coolTime = TableManager._instance.Takefloat(TableManager.eTableJsonNames.CharacterInfo, (int)nowPlayer, TableManager.eCharacterInfoIndex.CoolTime.ToString());
        _bulletSpeed = TableManager._instance.Takefloat(TableManager.eTableJsonNames.CharacterInfo, (int)nowPlayer, TableManager.eCharacterInfoIndex.Speed.ToString()) * playerSpeed;

        _prevTime = Time.time - _coolTime;
        _instanBullets = new List<Bullet>();
        _bulletPrefab = Resources.Load("PlayerCharacter/Bullets/" +  nowPlayer.ToString() + "Bullet") as GameObject;
    }

    public void Shoot()
    {
        if (_prevTime + _coolTime > Time.time) return;

        _prevTime = Time.time;

        int i = 0;
        while (i < _instanBullets.Count)
        {
            if (!_instanBullets[i].gameObject.activeSelf)
            {
                _instanBullets[i].transform.position = _bulletPos.position;
                _instanBullets[i].SpawnInit(_bulletSpeed, _player._sightDir, _damage);
                _instanBullets[i].gameObject.SetActive(true);
                return;
            }
            i++;
        }

        if (i == _instanBullets.Count)
        {
            GameObject go = Instantiate(_bulletPrefab, _bulletPos.position, Quaternion.identity);
            _instanBullets.Add(go.GetComponent<Bullet>());
            _instanBullets[i].SpawnInit(_bulletSpeed, _player._sightDir, _damage);
        }
    }
}
