using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class NeverStopManager : StageBaseManager
{
    [SerializeField] float _cameraOffsetX;

    Vector3 _spawnPos;
    BlockPooler _blockManager;
    FollowCamera _camSetting;

    bool _isNormalBlock;
    int _nowLevel;
    bool _speedUpTrigger;


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

                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    if (Input.GetKeyDown(KeyCode.Z))
                        _player._isSlide = true;
                    if (Input.GetKeyUp(KeyCode.Z))
                        _player._isSlide = false;
                    if (Input.GetKeyDown(KeyCode.M))
                        _player._isJump = true;
                    if (Input.GetKeyUp(KeyCode.Z))
                        _player._isJump = false;
                }
                _playTime += Time.deltaTime;
                _timer.SetTime(_playTime);
                _camSetting.Follow();

                if ((int)Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect * 2) > _blockManager._distance)
                {
                    _blockManager.DisableBlock();
                    _blockManager.InstanObj(_nowLevel, eNeverStopBlocks.EmptyMiddleBlock);
                    _isNormalBlock = !_isNormalBlock;
                }

                if (_speedUpTrigger)
                {
                    ProgPlay();
                    _speedUpTrigger = false;
                }
                if (_player._isDeath)
                {
                    ProgEnd();
                }
                break;

            case eGameState.Play:

                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    if (Input.GetKeyDown(KeyCode.Z))
                        _player._isSlide = true;
                    if (Input.GetKeyUp(KeyCode.Z))
                        _player._isSlide = false;
                    if (Input.GetKeyDown(KeyCode.M))
                        _player._isJump = true;
                    if (Input.GetKeyUp(KeyCode.Z))
                        _player._isJump = false;
                }

                _playTime += Time.deltaTime;
                _timer.SetTime(_playTime);
                _camSetting.Follow();


                if ((_playTime + 5) / 20 >= _nowLevel)
                {
                    StartCoroutine(ProgSpeedUp());
                }

                if ((int)Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect * 2) > _blockManager._distance)
                {
                    _blockManager.DisableBlock();

                    if (_isNormalBlock)
                    {
                        _blockManager.InstanObj(2, eNeverStopBlocks.EmptyMiddleBlock);
                    }
                    else
                    {
                        _blockManager.InstanObj(_nowLevel, (eNeverStopBlocks)Random.Range(1, (int)eNeverStopBlocks.Count));
                    }
                    _isNormalBlock = !_isNormalBlock;
                }

                if (_player._isDeath)
                {
                    ProgEnd();
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

        // 참조
        _blockManager = GameObject.Find("BlockPooler").GetComponent<BlockPooler>();
        _camSetting = Camera.main.gameObject.GetComponent<FollowCamera>();

        _playerSpawnPoint = GameObject.FindWithTag("Respawn").GetComponent<PlayerSpawnPoint>();
        //_timer = GameObject.FindGameObjectWithTag("UITimer").GetComponent<TimerBox>();
        _msgBox = GameObject.FindGameObjectWithTag("UIGameAlarmBox").GetComponent<AlarmBox>();

        // 초기 설정
        _player = _playerSpawnPoint.GeneratePlayer();


#if UNITY_EDITOR
        _player.Initialize(0, 6, new PlayerActive.PlayerIdle(), 7.5f, true);
#else
        _player.Initialize(0, 6, new PlayerActive.PlayerIdle(), 7.5f, true);
#endif
        _camSetting.SetUp(_player.transform, new Vector3(_cameraOffsetX, 0, 0), false, true);
        _blockManager.GetPrefabs();
        _blockManager.InstanObj(7, eNeverStopBlocks.EmptyMiddleBlock);
        _isNormalBlock = false;
        _nowLevel = 1;
        _speedUpTrigger = false;
        _msgBox.OpenBox("Ready!!");
        _playTime = 0;
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
    public IEnumerator ProgSpeedUp()
    {
        _nowGameState = eGameState.Event;


        yield return new WaitForSeconds(5.0f);

        SoundManager._instance.PlayEffect(eEffectSound.SpeedUp);
        _msgBox.OpenBox("Speed Up!!");

        _player._moveSpeedVal*=2;

        yield return new WaitForSeconds(1.0f);

        _msgBox.CloseBox();

        _speedUpTrigger = true;
        _nowLevel *= 2;
    }

    public override void ProgPlay()
    {
        _nowGameState = eGameState.Play;
        _player._moveDir = Vector3.right;
    }


    public override void ProgEnd(bool _isSuceess = false)
    {
        _nowGameState = eGameState.End;
        _msgBox.OpenBox("Game Over...");
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
