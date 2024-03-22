using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGenerator : MonoBehaviour
{
    [SerializeField] Transform[] _genPos;
    [SerializeField] Transform _slimeCollection;

    GameObject _enemyPrefab;     // 스폰할 때 사용할 레이저포인터 프리팹
    float _speed;
    float _hp;

    public void Init(float speed, float hp)
    {
        _enemyPrefab = Resources.Load("Prefab/DirtySlime") as GameObject;
        _speed = speed;
        _hp = hp;

        for (int i = 0; i < _genPos.Length; i++)
        {
            SlimeSpawn(_genPos[i].position);
        }
    }

    public bool UpdateStatus()
    {
        int deadSlime = 0;
        for (int i = 0; i < _slimeCollection.childCount; i++)
        {
            if (!_slimeCollection.GetChild(i).gameObject.activeSelf)
            {
                deadSlime++;
                continue;
            }
            Slime slime = _slimeCollection.GetChild(i).GetComponent<Slime>();

            if (slime._deadTirgger)
            {
                slime.transform.localScale = Vector3.zero;
                slime.gameObject.SetActive(false);
            }
            if (slime._divideTirgger)
            {
                slime._divideTirgger = false;
                SlimeDevide(slime);
            }
            slime.Move();
        }
        Debug.Log(deadSlime);
        Debug.Log(_slimeCollection.childCount);

        return (deadSlime == _slimeCollection.childCount);
    }

    void SlimeSpawn(Vector3 pos)
    {
        Slime slime = null;

        for (int i = 0; i < _slimeCollection.childCount; i++)
        {
            if (!_slimeCollection.GetChild(i).gameObject.activeSelf)
            {
                slime = _slimeCollection.GetChild(i).GetComponent<Slime>();
            }
        }

        if (slime == null)
        {
            slime = Instantiate(_enemyPrefab, _slimeCollection).GetComponent<Slime>();
        }

        slime.transform.position = pos;
        slime.Init(_speed, _hp, 4);
        slime.gameObject.SetActive(true);
    }

    void SlimeDevide(Slime mainSlime)
    {
        Slime subSlime = null;

        for (int i = 0; i < _slimeCollection.childCount; i++)
        {
            if (!_slimeCollection.GetChild(i).gameObject.activeSelf)
            {
                subSlime = _slimeCollection.GetChild(i).GetComponent<Slime>();
            }
        }
        
        if (subSlime == null)
        {
            subSlime = Instantiate(_enemyPrefab, _slimeCollection).GetComponent<Slime>();
        }


        subSlime.transform.position = mainSlime.transform.position;
        subSlime.Init(mainSlime._speedVal, mainSlime._maxHpVal, mainSlime._devideNumberVal);
        subSlime.gameObject.SetActive(true);

    }
}
