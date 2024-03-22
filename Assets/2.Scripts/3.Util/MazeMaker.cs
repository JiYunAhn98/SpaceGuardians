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
    Stack<Vector2Int> _stackDir = new Stack<Vector2Int>(); //지나온 길의 방향 저장
    public bool _changeFinishTrigger { get { return _changeFinish; } }

    public void MapSetting()
    {
        _mapList = new List<GameObject>();
        MakeMazeBackTracking(); // 처음 맵에는 spawnpoint와 switch가 존재
        MakeMazeEllers();       // 다음 맵에서는 exit만 존재

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

    // 재귀적 백트래킹 알고리즘
    /// <summary>
    /// 처음 만드는 경우에는 재귀적 백트래킹의 사용이 문제가 되지 않아서 사용해봤다.
    /// https://everycommit.tistory.com/16 참고
    /// </summary>
    public void MakeMazeBackTracking()
    {
        List<Vector3> SwitchPos;
        SwitchPos = new List<Vector3>();

        Init(); //미로 초기화
        RandPosSelect(); //시작위치 랜덤고르기
        GenerateRoad(5); //미로 생성
    }

    /// <summary>
    /// widthDis와 heightDis에 맵의 너비와 높이를 받으면 0.1을 곱하고 한 칸의 크기로 나눈다.
    /// </summary>
    private void Init()
    {
        _width = _widthDis / (int)(_cellSize * 10) + 3;
        _height = _heightDis / (int)(_cellSize * 10) + 3;  //+3인 이유 : width의 처음부터 끝까지가 미로맵에 해당하도록 하기 위하여 (양 끝 + 2, width의 마지막 부분까지 포함 +1 )

        _mapTmp = new int[_width, _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _mapTmp[x, y] = (int)TILE.WALL; //모든 타일을 벽으로 채움
            }
        }
    }

    void RandPosSelect()
    {
        do
        {
            _pos = new Vector2Int(1, 1); //미로 범위 내에서 무작위 선택
        } while (_pos.x % 2 == 0 || _pos.y % 2 == 0); //홀수 칸으로 설정
    }

    private void RandDirection() //무작위로 방향을 섞는 메소드
    {
        for (int i = 0; i < _direction.Length; i++)
        {
            int randNum = Random.Range(0, _direction.Length); //4방향 중 무작위로 방향 선택
            Vector2Int temp = _direction[randNum]; //현재 인덱스에 해당하는 방향과 랜덤으로 선택한 방향을 서로 바꿈
            _direction[randNum] = _direction[i];
            _direction[i] = temp;
        }
    }

    void GenerateRoad(int shortCut)
    {
        _mapTmp[_pos.x, _pos.y] = (int)TILE.START; //RandPosSelect 함수에서 무작위로 선택한 지점을 시작 지점으로 설정
        do
        {
            int index = -1; //-1은 갈 수 있는 길이 없음을 의미

            RandDirection(); //방향 무작위로 섞음

            for (int i = 0; i < _direction.Length; i++)
            {
                if (CheckForRoad(_pos, i))
                {
                    index = i; //선택한 방향에 길이 없을 경우 방향 배열의 인덱스 저장
                    break;
                }
            }

            if (index != -1) //갈 수 있는 길이 있을 경우
            {
                for (int i = 0; i < 2; i++) //길과 길 사이에 벽을 생성하기 위해 3칸을 길로 바꿈
                {
                    _stackDir.Push(_direction[index]); //스택에 방향 저장
                    _pos += _direction[index]; //위치 변수 수정
                    _mapTmp[_pos.x, _pos.y] = (int)TILE.CHECK; //타일 생성
                }
            }
            else //갈 수 있는 길이 없을 경우
            {
                for (int i = 0; i < 2; i++) //길을 만들 때 3칸을 만들었기 때문에 뒤로 돌아갈 때도 3칸 이동
                {
                    _mapTmp[_pos.x, _pos.y] = (int)TILE.ROAD; //완성된 길 의미
                    _pos += _stackDir.Pop() * -1; //방향을 저장하는 스택에서 데이터를 꺼낸뒤 -1을 곱해 방향 반전
                }
            }
        }
        while (_stackDir.Count != 0); //스택이 0이라는 것은 모든 길을 순회했다는 것을 의미하므로 반복문 종료

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
        if ((pos + _direction[index] * 2).x > _width - 2) return false; //2를 곱하는 이유는 길과 길 사이에 벽이 있기 때문
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

    // Ellers 알고리즘
    /// <summary>
    /// 스위치를 먹은 곳부터 새롭게 미로를 만든다. 미로생성에서 가장 빠른 알고리즘이라고 불리기 때문에 적합하다.
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
    /// 빈칸에 번호를 설정해 넣는다.
    /// </summary>
    void SetSectionNum()
    {
        int section = 1;
        for (int x = 1; x < _width - 1; x += 2)
        {
            // 홀수 부분이 WALL에 해당하면 섹션 부여
            for (int y = 1; y < _height - 1; y += 2)
            {
                if (_mapTmp[x, y] == (int)TILE.WALL) _mapTmp[x, y] = section++;
            }
        }
    }

    /// <summary>
    /// 벽을 허물기 위한 부분을 설정한다.
    /// </summary>
    void CreateRoad(int wallPercent)
    {
        // 다음 진행할 칸에 영역 넓히기
        // 이 부분을 같은 거를 모아놓고 한 군데를 뚫자
        for (int y = 1; y < _height - 2; y += 2)
        {
            // 각 section의 시작지점 임시저장
            int sectionStart = 1;
            int tmpX;

            for (int x = 3; x < _width; x += 2)
            {
                if (Random.Range(0, wallPercent) == 0)
                {
                    // y축 벽 뚫기
                    _mapTmp[x, y] = _mapTmp[x - 2, y];
                    _mapTmp[x - 1, y] = _mapTmp[x - 2, y];
                }
                else
                {
                    // x축 벽 뚫기, 같은 영역이라면 반드시 1개는 뚫어야 한다.
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
    /// 마지막 라인을 정리한다.
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
