using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class BlockPooler : MonoBehaviour
{
    GameObject[] _prefabs;
    Dictionary<eNeverStopBlocks, List<GameObject>> _spawnBlocks;
    Queue<GameObject> _nowActiveBlock;

    Vector3 _offset;

    public float _distance { get { return _offset.x; } }

    public void GetPrefabs()
    {
        _offset = new Vector3(0,0,0);
        _spawnBlocks = new Dictionary<eNeverStopBlocks, List<GameObject>>();
        _nowActiveBlock = new Queue<GameObject>();
        _prefabs = new GameObject[(int)eNeverStopBlocks.Count];

        for (int i = 0; i < _prefabs.Length; i++)
        {
            _prefabs[i] = Resources.Load("Prefab/NeverStopBlocks/" + ((eNeverStopBlocks)i).ToString()) as GameObject;
        }
    }

    public void InstanObj(int num, eNeverStopBlocks blockKey)
    {
        GameObject go = null;

        while (num > 0)
        {
            if (_spawnBlocks.ContainsKey(blockKey))
            {
                int i = 0;
                while (i < _spawnBlocks[blockKey].Count)
                {
                    if (!_spawnBlocks[blockKey][i].activeSelf)
                    {
                        go = _spawnBlocks[blockKey][i];
                        go.SetActive(true);
                        break;
                    }
                    i++;
                }

                if (i == _spawnBlocks[blockKey].Count)
                {
                    go = Instantiate(_prefabs[(int)blockKey], transform);
                }
            }
            else
            {
                _spawnBlocks.Add(blockKey, new List<GameObject>());
                go = Instantiate(_prefabs[(int)blockKey], transform);
            }

            go.transform.position = _offset;
            _spawnBlocks[blockKey].Add(go);
            _nowActiveBlock.Enqueue(go);
            num--;

            switch (blockKey)
            {
                case eNeverStopBlocks.FinishBlock:
                    _offset.x += 4;
                    return;
                case eNeverStopBlocks.DoubleFinishBlock:
                    _offset.x += 8;
                    return;
                default:
                    _offset.x += 4;
                    break;
            }

        }
    }

    public void DisableBlock()
    {
        while (_nowActiveBlock.Count > 10)
        {
            _nowActiveBlock.Peek().SetActive(false);
            _nowActiveBlock.Dequeue();
        }
    }
}
