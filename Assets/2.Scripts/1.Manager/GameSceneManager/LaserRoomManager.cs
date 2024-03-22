using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class LaserRoomManager : StageBaseManager
{
    // 참조 변수
    List<LaserPointer> _laserPointers;     //현재 게임에서 사용중인 레이저포인터
    GameObject _laserPrefab;            //스폰할 때 사용할 레이저포인터 프리팹

    // UI 참조 변수
    LevelGage _levelGage;
    JoyStick _joystick;

    // 정보 변수
    float _laserSpawnTime = 20;       // lazer spawn 시간
    float _laserSpawnNowTime;
    int _laserCount;

    float _laserArea = 4.5f;

    public override void UpdateFrame()
    {
        switch (_nowGameState)
        {
            case eGameState.Init:
                if (SceneControlManager._instance._loadEnd)
                {
                    StopAllCoroutines();
                    StartCoroutine(ProgReady());
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
                _playTime += Time.deltaTime;
                _levelGage.SetGage(_playTime, _laserSpawnTime);

                if (_levelGage._gageLevelUpgrade)
                {
                    _laserSpawnTime = 10;
                    foreach (LaserPointer laser in _laserPointers)
                    {
                        laser.UpgradeActive();
                    }
                }

                foreach (LaserPointer laser in _laserPointers)
                {
                    laser.LaserActive();
                }

                if (_playTime >= _laserSpawnNowTime)
                {
                    RandomLaserSpawn();
                    RandomLaserSpawn();
                    _laserSpawnNowTime += _laserSpawnTime;
                }

                // 죽으면 게임 끝
                if (_player._isDeath)
                    ProgEnd();

                _timer.SetTime(_playTime);
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


        // 참조
        _laserPrefab = (GameObject)Resources.Load("Prefab/LaserPointer");
        _levelGage = GameObject.Find("LevelGage").GetComponent<LevelGage>();
        _joystick = GameObject.Find("JoyStick").GetComponent<JoyStick>();
        _playerSpawnPoint = GameObject.FindWithTag("Respawn").GetComponent<PlayerSpawnPoint>();
        //_timer = GameObject.FindGameObjectWithTag("UITimer").GetComponent<TimerBox>();
        _msgBox = GameObject.FindGameObjectWithTag("UIGameAlarmBox").GetComponent<AlarmBox>();


        // 초기 설정
        _player = _playerSpawnPoint.GeneratePlayer();


#if UNITY_EDITOR
        _player.Initialize(0, 50, new PlayerActive.PlayerIdle());
#elif UNITY_ANDROID
        _player.Initialize(0, 4.5f, new PlayerActive.PlayerIdle());
#else
        _player.Initialize(0, 9f, new PlayerActive.PlayerIdle());
#endif
        _playTime = 0;
        _laserPointers = new List<LaserPointer>();
        _laserCount = 0;
        _laserSpawnNowTime = 0;
        _joystick.Init();
        _msgBox.OpenBox("Ready!!");
        _timer.OpenTimerBox(0);
    }
    public override IEnumerator ProgReady()
    {
        _nowGameState = eGameState.Ready;

        yield return new WaitForSeconds(2.0f);
        _msgBox.OpenBox("Start!!");
        yield return new WaitForSeconds(0.5f);

        ProgPlay();
    }
    public override void ProgPlay()
    {
        _nowGameState = eGameState.Play;
        _msgBox.CloseBox();
    }
    public override void ProgEnd(bool isSuccess = false)
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
    #endregion [Progress Start Functions]

    #region[내부함수]
    /// <summary>
    /// Lazer를 랜덤한 위치에 생성한다.
    /// </summary>
    void RandomLaserSpawn()
    {
        Vector3 lazerPos = Vector3.zero;
        Quaternion lazerRot = Quaternion.identity;

        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0:
                lazerPos = new Vector3(Random.Range(-_laserArea, _laserArea), -_laserArea, 0);
                break;
            case 1:
                lazerPos = new Vector3(Random.Range(-_laserArea, _laserArea), _laserArea, 0);
                lazerRot = Quaternion.Euler(new Vector3(0, 0, 180));
                break;
            case 2:
                lazerPos = new Vector3(-_laserArea, Random.Range(-_laserArea, _laserArea), 0);
                lazerRot = Quaternion.Euler(new Vector3(0, 0, 270));
                break;
            case 3:
                lazerPos = new Vector3(_laserArea, Random.Range(-_laserArea, _laserArea), 0);
                lazerRot = Quaternion.Euler(new Vector3(0, 0, 90));
                break;
        }

        GameObject go = Instantiate(_laserPrefab, lazerPos, lazerRot, transform);

        _laserPointers.Add(go.GetComponent<LaserPointer>());

        if (_levelGage._gageLevelUpgrade)
        {
            _laserPointers[_laserCount].Initialize(true);
        }
        else
        {
            _laserPointers[_laserCount].Initialize(false);
        }

        _laserCount++;
    }
    #endregion[내부함수]

    //private void OnGUI()
    //{
    //    GUIStyle uIStyle = new GUIStyle();
    //    uIStyle.fontSize = 50;
    //    uIStyle.normal.textColor = Color.red;
    //    GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "_nowGameState : " + _nowGameState.ToString(), uIStyle);
    //    GUI.Label(new Rect(0, 50, Screen.width, Screen.height), "_laserPrefab : " + _laserPrefab, uIStyle);
    //    GUI.Label(new Rect(0, 100, Screen.width, Screen.height), "_joystick : " + _joystick, uIStyle);
    //    GUI.Label(new Rect(0, 150, Screen.width, Screen.height), "_levelGage : " + _levelGage, uIStyle);
    //    GUI.Label(new Rect(0, 200, Screen.width, Screen.height), "_timer : " + _timer, uIStyle);
    //    GUI.Label(new Rect(0, 250, Screen.width, Screen.height), "Time : " + _timer.GetTime(), uIStyle);
    //}
}
