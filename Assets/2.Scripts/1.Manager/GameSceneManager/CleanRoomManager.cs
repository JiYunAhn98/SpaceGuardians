using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class CleanRoomManager : StageBaseManager
{
    // 참조 변수
    SlimeGenerator _slimeController;

    // UI 참조 변수
    JoyStick _joystick;
    JoyStick _shootJoystick;
    float _totalTime = 180;
    bool _isSuccess;

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
                // 시간이 흐르면서 이루어지는 과정
                _playTime -= Time.deltaTime;
                _player.Shoot();
                _joystick.PCMovementUpdate();
                if (_slimeController.UpdateStatus())
                {
                    ProgEnd(true);
                }
                if (_player._isDeath || 0 >= _playTime)
                { 
                    ProgEnd(false);
                }
                _timer.SetTime(_playTime);
                break;
            case eGameState.End:
                SoundManager._instance.BGMSoundDown(Time.deltaTime);

                if (SoundManager._instance._soundVolume <= 0.5f)
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
        //_timer = GameObject.FindGameObjectWithTag("UITimer").GetComponent<TimerBox>();
        _msgBox = GameObject.FindGameObjectWithTag("UIGameAlarmBox").GetComponent<AlarmBox>();
        _joystick = GameObject.Find("MoveJoyStick").GetComponent<JoyStick>();
        _shootJoystick = GameObject.Find("ShootJoyStick").GetComponent<JoyStick>();
        _slimeController = GameObject.Find("SlimeGenerator").GetComponent<SlimeGenerator>();

        _player = _playerSpawnPoint.GeneratePlayer();

        _joystick.Init();
        _shootJoystick.SightInit();
#if UNITY_EDITOR
        _player.Initialize(0, 40, new PlayerActive.PlayerIdle(), 0, false, true);
#elif UNITY_ANDROID
        _player.Initialize(0, 4F, new PlayerActive.PlayerIdle(), 0, false, true);
#else
        _player.Initialize(0, 8, new PlayerActive.PlayerIdle(), 0, false, true);
#endif
        _slimeController.Init(1, 10);

        _playTime = _totalTime;
        _timer.OpenTimerBox(0);
    }
    public override IEnumerator ProgReady()
    {
        _nowGameState = eGameState.Ready;

        _msgBox.OpenBox("Ready!!");
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
        _isSuccess = isSuceess;

        if (isSuceess)
        {
            _msgBox.OpenBox("Mission Clear!!");
        }
        else
        {
            _msgBox.OpenBox("Game Over...");
        }
    }
    public override IEnumerator ProgResult()
    {
        _nowGameState = eGameState.Result;
        yield return new WaitForSeconds(1f);
        _msgBox.CloseBox();

        if (SceneControlManager._instance._returnScene == eSceneName.Stadium)
        {
            if (_isSuccess)
            {
                SceneControlManager._instance._resultScore = (int)_playTime;
            }
            else
            {
                SceneControlManager._instance._resultScore = 0;
            }
            SceneControlManager._instance.LoadStadiumNextScene();
        }
        else
        {
            UIManager._instance.OpenPopup(ePopup.ResultWnd);
        }
    }
}
