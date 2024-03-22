using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class JumpClimbRoomManager : StageBaseManager
{
    [SerializeField] GameObject[] _objectStairPrefabs;
    [SerializeField] GameObject[] _objectObstaclePrefabs;

    int _obtacleDistance = 5;
    float _totalTime = 120;

    GameObject[] _objectsSpawnPoints;


    JoyStick _joystick;
    List<Block> _stairs;
    List<GameObject> _obstacles;
    FollowCamera _camSetting;


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

                _joystick.PCMovementUpdate();
                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                        _player._isJump = true;
                    if (Input.GetKeyUp(KeyCode.Space))
                        _player._isJump = false;
                }
                // 시간이 흐르면서 이루어지는 과정
                _playTime -= Time.deltaTime;
                _timer.SetTime(_playTime);
                _camSetting.Follow();

                // 죽으면 게임 끝
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
    public override void ProgInit()
    {
        _nowGameState = eGameState.Init;

        _playerSpawnPoint = GameObject.FindWithTag("Respawn").GetComponent<PlayerSpawnPoint>();
        _timer = GameObject.FindGameObjectWithTag("UITimer").GetComponent<TimerBox>();
        _msgBox = GameObject.FindGameObjectWithTag("UIGameAlarmBox").GetComponent<AlarmBox>();

        _player = _playerSpawnPoint.GeneratePlayer();

#if UNITY_EDITOR
        _player.Initialize(int.MaxValue, 5, new PlayerActive.PlayerIdle(), 8.7f, true);
#else
        _player.Initialize(int.MaxValue, 5, new PlayerActive.PlayerIdle(), 8.7f, true);
#endif
        _timer.OpenTimerBox(0);

        _stairs = new List<Block>();
        _obstacles = new List<GameObject>();

        _objectsSpawnPoints = GameObject.FindGameObjectsWithTag("ObjectSpawnPoint");

        _joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<JoyStick>();
        _camSetting = Camera.main.gameObject.GetComponent<FollowCamera>();

        float PosX = 0;
        for (int i = 0; i < _objectsSpawnPoints.Length; i++)
        {
            int num = Random.Range(0, _objectStairPrefabs.Length);
            GameObject go = Instantiate(_objectStairPrefabs[num], _objectsSpawnPoints[i].transform);
            float distance = Random.Range(-2,3) * 2.5f;

            PosX += distance;
            if (PosX < -5)
                PosX = -5;
            if (PosX > 5)
                PosX = 5;
            go.transform.position += Vector3.right * PosX;
            _stairs.Add(go.GetComponent<Block>());
            _stairs[i].Init();

            if (i % _obtacleDistance == 0)
            {
                num = Random.Range(0, _objectObstaclePrefabs.Length);
                go = Instantiate(_objectObstaclePrefabs[num], _objectsSpawnPoints[i].transform);
                go.transform.position += Vector3.up * 1.5f;
            }
        }

        _joystick.Init();
        _camSetting.SetUp(_player.transform, Vector3.zero, true, false);
        _msgBox.OpenBox("Ready!!");
        _playTime = _totalTime;
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
    }
    public override void ProgEnd(bool isSuceess)
    {
        _nowGameState = eGameState.End;

        if (isSuceess)
        {
            _msgBox.OpenBox("Goal In!!");
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
}
