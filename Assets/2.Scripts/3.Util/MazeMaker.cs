using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.RendererUtils;

enum TILE { SWITCH = -1, WALL = 0, ROAD, CHECK, START };
public class MazeMaker : MonoBehaviour
{
    [SerializeField] int _widthDis = 42;
    [SerializeField] int _heightDis = 42;
    [SerializeField] float _cellSize = 0.3f;
    [SerializeField] GameObject _wallPrefab;
    [SerializeField] GameObject _startPrefab;
    [SerializeField] GameObject _switchPrefab;
    [SerializeField] GameObject _finishPrefab;
    [SerializeField] GameObject _numberTmp;

    [SerializeField] PostProcessVolume _ppv;
    int _width;
    int _height;
    int[,] _mapTmp;
    bool _changeFinish;

    List<GameObject> _mapList;
    int _nowMapIndex;

    Vector2Int[] _direction = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    Vector2Int _pos = Vector2Int.zero;
    Stack<Vector2Int> _stackDir = new Stack<Vector2Int>(); //������ ���� ���� ����
    public bool _changeFinishTrigger { get { return _changeFinish; } }

    public void MapSetting()
    {
        _mapList = new List<GameObject>();
        MakeMazeBackTracking(); // ó�� �ʿ��� spawnpoint�� switch�� ����
        MakeMazeEllers();       // ���� �ʿ����� exit�� ����

        _nowMapIndex = 0;
        _mapList[_nowMapIndex].SetActive(true);
    }

    public void MapChange()
    {
        if (_nowMapIndex + 1 >= _mapList.Count) return;

        _mapList[_nowMapIndex].SetActive(false);
        _nowMapIndex++;
        _mapList[_nowMapIndex].SetActive(true);

        StartCoroutine(PostProcessEffect());
    }
    public IEnumerator PostProcessEffect()
    {
        FollowCamera followCamera = Camera.main.gameObject.GetComponent<FollowCamera>();

        Vector3 leftVec = new Vector3(-0.02f, 0, 0);
        Vector3 rightVec = new Vector3(0.02f, 0, 0);
        _ppv.profile.GetSetting<Vignette>().intensity.value = 1;
        _ppv.profile.GetSetting<Vignette>().smoothness.value = 1;

        followCamera._offsetSetting += rightVec;
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.1f);
            followCamera._offsetSetting += leftVec;
            followCamera._offsetSetting += leftVec;
            yield return new WaitForSeconds(0.1f);
            followCamera._offsetSetting += rightVec;
            followCamera._offsetSetting += rightVec;
        }
        followCamera._offsetSetting += leftVec;
        yield return new WaitForSeconds(1f);

        _ppv.profile.GetSetting<Vignette>().intensity.value = 0.6f;
        _ppv.profile.GetSetting<Vignette>().smoothness.value = 0.6f;

        _changeFinish = true;

        yield break;
    }

    // ����� ��Ʈ��ŷ �˰���
    /// <summary>
    /// ó�� ����� ��쿡�� ����� ��Ʈ��ŷ�� ����� ������ ���� �ʾƼ� ����غô�.
    /// https://everycommit.tistory.com/16 ����
    /// </summary>
    public void MakeMazeBackTracking()
    {
        List<Vector3> SwitchPos;
        SwitchPos = new List<Vector3>();

        Init(); //�̷� �ʱ�ȭ
        RandPosSelect(); //������ġ ��������
        GenerateRoad(5); //�̷� ����
    }

    /// <summary>
    /// widthDis�� heightDis�� ���� �ʺ�� ���̸� ������ 0.1�� ���ϰ� �� ĭ�� ũ��� ������.
    /// </summary>
    private void Init()
    {
        _width = _widthDis / (int)(_cellSize * 10) + 3;
        _height = _heightDis / (int)(_cellSize * 10) + 3;  //+3�� ���� : width�� ó������ �������� �̷θʿ� �ش��ϵ��� �ϱ� ���Ͽ� (�� �� + 2, width�� ������ �κб��� ���� +1 )

        _mapTmp = new int[_width, _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _mapTmp[x, y] = (int)TILE.WALL; //��� Ÿ���� ������ ä��
            }
        }
    }

    void RandPosSelect()
    {
        do
        {
            _pos = new Vector2Int(1, 1); //�̷� ���� ������ ������ ����
        } while (_pos.x % 2 == 0 || _pos.y % 2 == 0); //Ȧ�� ĭ���� ����
    }

    private void RandDirection() //�������� ������ ���� �޼ҵ�
    {
        for (int i = 0; i < _direction.Length; i++)
        {
            int randNum = Random.Range(0, _direction.Length); //4���� �� �������� ���� ����
            Vector2Int temp = _direction[randNum]; //���� �ε����� �ش��ϴ� ����� �������� ������ ������ ���� �ٲ�
            _direction[randNum] = _direction[i];
            _direction[i] = temp;
        }
    }

    void GenerateRoad(int shortCut)
    {
        _mapTmp[_pos.x, _pos.y] = (int)TILE.START; //RandPosSelect �Լ����� �������� ������ ������ ���� �������� ����
        do
        {
            int index = -1; //-1�� �� �� �ִ� ���� ������ �ǹ�

            RandDirection(); //���� �������� ����

            for (int i = 0; i < _direction.Length; i++)
            {
                if (CheckForRoad(_pos, i))
                {
                    index = i; //������ ���⿡ ���� ���� ��� ���� �迭�� �ε��� ����
                    break;
                }
            }

            if (index != -1) //�� �� �ִ� ���� ���� ���
            {
                for (int i = 0; i < 2; i++) //��� �� ���̿� ���� �����ϱ� ���� 3ĭ�� ��� �ٲ�
                {
                    _stackDir.Push(_direction[index]); //���ÿ� ���� ����
                    _pos += _direction[index]; //��ġ ���� ����
                    _mapTmp[_pos.x, _pos.y] = (int)TILE.CHECK; //Ÿ�� ����
                }
            }
            else //�� �� �ִ� ���� ���� ���
            {
                for (int i = 0; i < 2; i++) //���� ���� �� 3ĭ�� ������� ������ �ڷ� ���ư� ���� 3ĭ �̵�
                {
                    _mapTmp[_pos.x, _pos.y] = (int)TILE.ROAD; //�ϼ��� �� �ǹ�
                    _pos += _stackDir.Pop() * -1; //������ �����ϴ� ���ÿ��� �����͸� ������ -1�� ���� ���� ����
                }
            }
        }
        while (_stackDir.Count != 0); //������ 0�̶�� ���� ��� ���� ��ȸ�ߴٴ� ���� �ǹ��ϹǷ� �ݺ��� ����

        OnDrawTile("BackTrackingMap");
    }
    bool isDoubleWall(Vector2Int pos)
    {
        for (int i = 0; i < 2; i++)
        {
            Vector2Int tmp1 = pos + _direction[i];
            Vector2Int tmp2 = pos - _direction[i];
            if (_mapTmp[tmp1.x, tmp1.y] == (int)TILE.WALL && _mapTmp[tmp2.x, tmp2.y] == (int)TILE.WALL)
            {
                return true;
            }
        }
        return false;
    }
    bool CheckForRoad(Vector2Int pos, int index)
    {
        if ((pos + _direction[index] * 2).x > _width - 2) return false; //2�� ���ϴ� ������ ��� �� ���̿� ���� �ֱ� ����
        if ((pos + _direction[index] * 2).x < 0) return false;
        if ((pos + _direction[index] * 2).y > _height - 2) return false;
        if ((pos + _direction[index] * 2).y < 0) return false;
        if (_mapTmp[(pos + _direction[index] * 2).x, (pos + _direction[index] * 2).y] != (int)TILE.WALL) return false;
        return true;
    }
    private void OnDrawTile(string name)
    {
        GameObject go = new GameObject(name);
        go.transform.position = Vector3.zero;
        go.transform.parent = gameObject.transform;

        for (int x = 1; x < _width - 1; x++)
        {
            for (int y = 1; y < _height - 1; y++)
            {
                Vector3 tmp = new Vector3(_widthDis / 20.0f - ((x-1) * _cellSize), _heightDis / 20.0f - ((y-1) * _cellSize), 0);
                switch (_mapTmp[x, y])
                {
                    case (int)TILE.START:
                        break;
                    case (int)TILE.WALL:
                        Instantiate(_wallPrefab, tmp, Quaternion.identity, go.transform);
                        break;
                }
            }
        }

        _mapList.Add(go);
        go.SetActive(false);
    }

    // Ellers �˰���
    /// <summary>
    /// ����ġ�� ���� ������ ���Ӱ� �̷θ� �����. �̷λ������� ���� ���� �˰����̶�� �Ҹ��� ������ �����ϴ�.
    /// </summary>
    public void MakeMazeEllers()
    {
        Init();
        SetSectionNum();
        CreateRoad(2);
        LastLineMerge();

        OnDrawTile("EllersMap");
    }

    /// <summary>
    /// ��ĭ�� ��ȣ�� ������ �ִ´�.
    /// </summary>
    void SetSectionNum()
    {
        int section = 1;
        for (int x = 1; x < _width - 1; x += 2)
        {
            // Ȧ�� �κ��� WALL�� �ش��ϸ� ���� �ο�
            for (int y = 1; y < _height - 1; y += 2)
            {
                if (_mapTmp[x, y] == (int)TILE.WALL) _mapTmp[x, y] = section++;
            }
        }
    }

    /// <summary>
    /// ���� �㹰�� ���� �κ��� �����Ѵ�.
    /// </summary>
    void CreateRoad(int wallPercent)
    {
        // ���� ������ ĭ�� ���� ������
        // �� �κ��� ���� �Ÿ� ��Ƴ��� �� ������ ����
        for (int y = 1; y < _height - 2; y += 2)
        {
            // �� section�� �������� �ӽ�����
            int sectionStart = 1;
            int tmpX;

            for (int x = 3; x < _width; x += 2)
            {
                if (Random.Range(0, wallPercent) == 0)
                {
                    // y�� �� �ձ�
                    _mapTmp[x, y] = _mapTmp[x - 2, y];
                    _mapTmp[x - 1, y] = _mapTmp[x - 2, y];
                }
                else
                {
                    // x�� �� �ձ�, ���� �����̶�� �ݵ�� 1���� �վ�� �Ѵ�.
                    tmpX = Random.Range(sectionStart, x);
                    tmpX = (tmpX & 1) == 1 ? tmpX : tmpX - 1;

                    while ((tmpX & 1) == 1)
                    {
                        _mapTmp[tmpX, y+1] = _mapTmp[tmpX, y];
                        _mapTmp[tmpX, y+2] = _mapTmp[tmpX, y];

                        tmpX = Random.Range(sectionStart, x);
                    }
                    sectionStart = x;
                }
            }

            tmpX = Random.Range(sectionStart, _width);
            tmpX = (tmpX & 1) == 1 ? tmpX : tmpX - 1;

            _mapTmp[tmpX, y + 1] = _mapTmp[tmpX, y];
            _mapTmp[tmpX, y + 2] = _mapTmp[tmpX, y];
        }
    }

    /// <summary>
    /// ������ ������ �����Ѵ�.
    /// </summary>
    void LastLineMerge()
    {
        int lastY = _height - 2;

        for (int x = 2; x <= _width - 2; x += 2)
        {
            if (_mapTmp[x-1, lastY] != _mapTmp[x+1, lastY])
            {
                _mapTmp[x, lastY] = 1;
            }
        }
    }

}
