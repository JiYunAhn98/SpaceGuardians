using System.Collections;
using System.Collections.Generic;
using DefineHelper;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SprintRaceManager : StageBaseManager
{
    // 한 발자국 당 오브젝트 크기가 1씩 줄어들고 3이하가 되면 사라지도록하자
    // spike 10걸음 전에 화살표 알림이 뜬다. 그리고 5걸음부터 사라지고 5걸음 뒤에는 점프버튼 등장
    // 만약 점프버튼이 나왔는데 다른클릭을 하면 경직3초
    // 설정 변수
    [SerializeField] int _runMaxDis = 500;
    [SerializeField] int _spikeMaxCnt = 10;
    [SerializeField] int _alarmDis = 20;
    float _totalTime = 120;

    // Resource
    GameObject _spikeAlarm;
    GameProgressBox _distanceBox;

    // 정보 변수
    bool _isOnLeftFootBtn;      // 왼발인지 체크
    bool _isOnRightFootBtn;     // 오른발인지 체크
    int[] _spikeTiming;
    int _runDistance;
    int _nowSpikeCnt;
    int _appearCnt;
    KeyCode _key;
    bool _stepClear;

    // 생성된 오브젝트
    [SerializeField] GameObject[] _spike;
    [SerializeField] GameObject[] _finishLine;
    [SerializeField] GameObject _jumpBtn;
    [SerializeField] GameObject _hitBox;

    public override void UpdateFrame()
    {
        switch (_nowGameState)
        {
            case eGameState.Init:
                if (SceneControlManager._instance._loadEnd)
                {
                    StartCoroutine(ProgReady());

                }
                break;
            case eGameState.Play:

                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    if (Input.GetKeyDown(KeyCode.M))
                        UpStepCount(false);
                    else if (Input.GetKeyDown(KeyCode.Z))
                        UpStepCount(true);
                }

                // 시간이 흐르면서 이루어지는 과정
                _playTime -= Time.deltaTime;
                _timer.SetTime( _playTime);

                if (_isOnRightFootBtn != _isOnLeftFootBtn)
                {
                    _isOnRightFootBtn = _isOnLeftFootBtn;
                    if (_runDistance > _runMaxDis - 3)
                    {
                        if (_appearCnt >= 0)
                        {
                            _finishLine[_appearCnt].SetActive(false);
                        }
                        _appearCnt++;
                        _finishLine[_appearCnt].SetActive(true);
                        if (_appearCnt >= 2)
                        {
                            ProgEnd(true);
                        }
                    }
                    else if (_nowSpikeCnt < _spikeTiming.Length)
                    {
                        if (_runDistance >= _spikeTiming[_nowSpikeCnt] && _jumpBtn.activeSelf)
                        {
                            _jumpBtn.SetActive(false);
                            _spike[_appearCnt].SetActive(false);
                            _appearCnt = -1;
                            _nowSpikeCnt++;
                            _hitBox.SetActive(true);
                        }
                        else if (_runDistance > _spikeTiming[_nowSpikeCnt] - 4)
                        {
                            _hitBox.SetActive(false);
                            if (_appearCnt >= 0)
                            {
                                _spike[_appearCnt].SetActive(false);
                            }
                            _appearCnt++;
                            _spikeAlarm.SetActive(false);
                            _spike[_appearCnt].SetActive(true);
                            if (_appearCnt >= 2)
                            {
                                JumpTurn();
                            }
                        }
                        else if (_runDistance >= _spikeTiming[_nowSpikeCnt] - _alarmDis)
                        {
                            _hitBox.SetActive(false);
                            _spikeAlarm.SetActive(true);
                        }
                        else
                        {
                            _appearCnt = -1;
                            _jumpBtn.SetActive(false);
                            _hitBox.SetActive(false);
                        }
                    }
                }

                if (_player._isDeath || _playTime <= 0)
                {
                    ProgEnd(false);
                }
                break;
            case eGameState.End:
                SoundManager._instance.BGMSoundDown(Time.deltaTime);

                if (SoundManager._instance._soundVolume <= 0.2f)
                {
                    StopAllCoroutines();
                    StartCoroutine(ProgResult());
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        _stepClear = false;
    }

    #region[Progress Start Functions]
    public override void ProgInit()
    {
        _nowGameState = eGameState.Init;

        _isOnLeftFootBtn = true;
        _isOnRightFootBtn = false;

        _spikeTiming = new int[_spikeMaxCnt];
        int section = (_runMaxDis - 100) / _spikeMaxCnt + 2; // 40
        int cnt = 0;
        for (int distance = 50; distance < _runMaxDis - 50; distance += section)
        {
            _spikeTiming[cnt++] = Random.Range(distance + _alarmDis, distance + section);
        }

        _distanceBox = GameObject.FindGameObjectWithTag("UIGameProgressBox").GetComponent<GameProgressBox>();
        _spikeAlarm = GameObject.Find("ObjectAlarm");
        _spikeAlarm.SetActive(false);
        _hitBox.SetActive(false);

        for (int i = 0; i < _spike.Length; i++)
        {
            _spike[i].SetActive(false);
            _finishLine[i].SetActive(false);
        }

        _playerSpawnPoint = GameObject.FindWithTag("Respawn").GetComponent<PlayerSpawnPoint>();
        //_timer = GameObject.FindGameObjectWithTag("UITimer").GetComponent<TimerBox>();
        _msgBox = GameObject.FindGameObjectWithTag("UIGameAlarmBox").GetComponent<AlarmBox>();

        _player = _playerSpawnPoint.GeneratePlayer();
        _player.Initialize(int.MaxValue, 0, PlayerActive.PlayerIdle._instance, 3, true);
        _playTime = _totalTime;

        _jumpBtn.gameObject.SetActive(false);
        _appearCnt = -1;
        _distanceBox.SetDistance(_runMaxDis);
        _msgBox.OpenBox("Ready!!");
        _timer.OpenTimerBox(0);
    }
    public override IEnumerator ProgReady()
    {
        _nowGameState = eGameState.Ready;

        yield return new WaitForSeconds(2.0f);
        _msgBox.OpenBox("Start!!");
        yield return new WaitForSeconds(0.5f);
        _msgBox.CloseBox();

        ProgPlay();
    }
    public override void ProgPlay()
    {
        _nowGameState = eGameState.Play;

        _nowSpikeCnt = 0;
        _runDistance = 0;

    }
    public override void ProgEnd(bool isSuccess)
    {
        _nowGameState = eGameState.End;

        if (isSuccess)
        {
            _msgBox.OpenBox("Goal In !!");
        }
        else
        {
            _msgBox.OpenBox("Failed...");
        }
    }
    public override IEnumerator ProgResult()
    {
        _nowGameState = eGameState.Result;
        yield return new WaitForSeconds(1f);

        if (SceneControlManager._instance._returnScene == eSceneName.Stadium)
        {
            SceneControlManager._instance._resultScore = (int)_playTime;
            SceneControlManager._instance.LoadStadiumNextScene();
        }
        else
        {
            UIManager._instance.OpenPopup(ePopup.ResultWnd);
        }
    }
#endregion [Progress Start Functions]

    public void UpStepCount(bool trigger)
    {
        if (_stepClear || _player._isJump || _player._isFall || _player._isHit || _runDistance >= _runMaxDis) return;
        _stepClear = true;
        if (trigger != _isOnLeftFootBtn || _runDistance < 0)
        {
            _player._moveDir = Vector3.down;
            _isOnLeftFootBtn = trigger;
            _runDistance++;
            _distanceBox.SetDistance(_runMaxDis - _runDistance);
            _player.transform.localPosition = Vector3.zero;
        }
        else
        {
            _player._moveDir = Vector3.zero;
        }
    }

    public void JumpTurn()
    {
        _jumpBtn.gameObject.SetActive(true);
        RectTransform rect = _jumpBtn.GetComponent<RectTransform>();

        int x = Random.Range(0, 1250);
        int y = Random.Range(0, 550);

        rect.offsetMin = new Vector2(x, y);
        rect.offsetMax = new Vector2(x - 1250, y - 550);
    }
    public void JumpBtn()
    {
        _spike[_appearCnt].SetActive(false);
        _jumpBtn.SetActive(false);
        _player._isJump = true;

        _nowSpikeCnt++;
        _runDistance++;
        _appearCnt = -1;
        _distanceBox.SetDistance(_runMaxDis - _runDistance);
    }
    public IEnumerator SpikeGenerate()
    {
        _spikeAlarm.SetActive(true);
        yield return 0;
        _spikeAlarm.SetActive(false);

        for (int cnt = 0; cnt < _spike.Length; cnt++)
        {
            _spike[cnt].SetActive(true);
            yield return 0;
            _spike[cnt].SetActive(false);
        }

        yield break;

    }
    public IEnumerator FinishLineGenerate()
    {
        for (int cnt = 0; cnt < _finishLine.Length; cnt++)
        {
            _finishLine[cnt].SetActive(true);
            yield return 0;
            _finishLine[cnt].SetActive(false);
        }
        yield break;
    }
}
