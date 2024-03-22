using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.RendererUtils;

public class EscapeRoomManager : StageBaseManager
{
    [SerializeField] MazeMaker _MazeMaker;
    [SerializeField] GameObject _outTrigger;
    [SerializeField] PostProcessVolume _ppv;
    [SerializeField] JoyStick _joystick;
    [SerializeField] HeartRecoverSystem _heartRecover;
    [SerializeField] GhostGenerator _ghostGen;

    float _totalTime = 150;
    bool _isSuccess;

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
            case eGameState.Event:
                _playTime -= Time.deltaTime;
                _timer.SetTime(_playTime);
                _camSetting.Follow();
                
                if (_heartRecover.UpdateSystem())
                {
                    ProgPlay();
                }
                // 죽으면 게임 끝
                if (_player._isDeath || 0 >= _playTime)
                {
                    ProgEnd(false);
                }
                break;

            case eGameState.Play:
                _joystick.PCMovementUpdate();
                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                        _player._isRoll = true;
                }
                // 시간이 흐르면서 이루어지는 과정
                _playTime -= Time.deltaTime;
                _timer.SetTime(_playTime);
                _camSetting.Follow();
                _ghostGen.GhostsMove();

                // 죽으면 게임 끝
                if (_player._isDeath || 0 >= _playTime)
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

    #region[Progress Start Functions]
    public override void ProgInit()
    {
        _nowGameState = eGameState.Init;

        _playerSpawnPoint = GameObject.FindGameObjectWithTag("Respawn").GetComponent<PlayerSpawnPoint>();
        //_timer = GameObject.FindGameObjectWithTag("UITimer").GetComponent<TimerBox>();
        _msgBox = GameObject.FindGameObjectWithTag("UIGameAlarmBox").GetComponent<AlarmBox>();
        //_joystick = GameObject.FindGameObjectWithTag("JoyStick").GetComponent<JoyStick>();
        _camSetting = Camera.main.gameObject.GetComponent<FollowCamera>();
        _ghostGen.GhostInit();

        _MazeMaker.MapSetting();
        _player = _playerSpawnPoint.GeneratePlayer();
        _camSetting.SetUp(_player.transform, Vector3.zero);
        _joystick.Init();
        _playTime = _totalTime;
        _heartRecover.InitSystem();
#if UNITY_EDITOR
        _player.Initialize(100, 10, new PlayerActive.PlayerIdle());
#elif UNITY_ANDROID
        _player.Initialize(100, 1f, new PlayerActive.PlayerIdle());
#else
        _player.Initialize(100, 2f, new PlayerActive.PlayerIdle());
#endif
    }
    public void ProgMazeSetting()
    {
        _nowGameState = eGameState.Event;

        SoundManager._instance.PlayEffect(eEffectSound.MazeChange);
        _MazeMaker.MapChange();
        _ghostGen.GhostsAllActive();
        _outTrigger.SetActive(true);
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
        _joystick._stopTrigger = false;
    }
    public void ProgHeal()
    {
        _nowGameState = eGameState.Event;
        _heartRecover.OpenSystem();
        _joystick._stopTrigger = true;
    }
    public override void ProgEnd(bool isSuccess = true)
    {
        _nowGameState = eGameState.End;
        _isSuccess = isSuccess;

        if (isSuccess)
        {
            _msgBox.OpenBox("Success!!");
            StartCoroutine(PostProcessing(0, 0));
        }
        else
        {
            _msgBox.OpenBox("Failed..");
        }
    }
    public IEnumerator PostProcessing(int intensity, int smoothness)
    {
        while (_ppv.profile.GetSetting<Vignette>().intensity.value > intensity && _ppv.profile.GetSetting<Vignette>().smoothness.value > intensity)
        {
            _ppv.profile.GetSetting<Vignette>().intensity.value -= Time.deltaTime;
            _ppv.profile.GetSetting<Vignette>().smoothness.value -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }
    public override IEnumerator ProgResult()
    {
        _nowGameState = eGameState.Result;
        yield return new WaitForSeconds(1f);

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
    #endregion [Progress Start Functions]


    //private void OnGUI()
    //{
    //    GUIStyle uIStyle = new GUIStyle();
    //    uIStyle.fontSize = 50;
    //    uIStyle.normal.textColor = Color.red;
    //    GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "_nowGameState : " + _nowGameState.ToString(), uIStyle);
    //    GUI.Label(new Rect(0, 50, Screen.width, Screen.height), "_timer : " + _timer, uIStyle);
    //    GUI.Label(new Rect(0, 100, Screen.width, Screen.height), "Time : " + _timer.GetTime(), uIStyle);
    //    GUI.Label(new Rect(0, 150, Screen.width, Screen.height), "_joystick : " + _joystick, uIStyle);
    //}
}
