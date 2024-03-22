using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
public class SpaceShipManager : SceneBaseManager
{
    // 참조 Resource
    GameObject _slotPrefab;

    // 참조 변수
    StageSelectTrigger _stageEnterTriggerZone;
    PlayerSpawnPoint _playerSpawnPoint;
    Player _player;
    FollowCamera _camSetting;

    //정보변수
    Vector3 _goalPos;

    public override void UpdateFrame()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (Input.GetMouseButton(0))
        {
            _goalPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        }
#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            _goalPos =  Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, -Camera.main.transform.position.z));
        }
#endif
        if ((_goalPos - _player.transform.position).magnitude <= 0.1f || _goalPos.y >= 1.8f || _goalPos.x > 3 || _goalPos.x < -3)
            _player._moveDir = Vector3.zero;
        else
            _player._moveDir = _goalPos - _player.transform.position;


        _camSetting.Follow();
    }

    public override void ProgInit()
    {
        // 리소스 Load
        _slotPrefab = Resources.Load("UI/StageSlot/SpaceShipStageSlot") as GameObject;

        // 초기 참조
        _playerSpawnPoint = GameObject.FindGameObjectWithTag("Respawn").GetComponent<PlayerSpawnPoint>();
        _stageEnterTriggerZone = GameObject.Find("StageListTrigger").GetComponent<StageSelectTrigger>();

        // Spaceship에서 해당되는 player의 움직임
        _player = _playerSpawnPoint.GeneratePlayer();
#if UNITY_EDITOR
        _player.Initialize(0, 30, new PlayerActive.PlayerIdle());
#elif UNITY_ANDROID
        _player.Initialize(0, 3, new PlayerActive.PlayerIdle());
#else
        _player.Initialize(0, 6, new PlayerActive.PlayerIdle());
#endif
        // 초기 설정
        _camSetting = Camera.main.gameObject.GetComponent<FollowCamera>();
        _camSetting.SetUp(_player.transform, Vector3.zero);
        _stageEnterTriggerZone.LoadSlots(_slotPrefab);
        _stageEnterTriggerZone.transform.GetChild(0).gameObject.SetActive(false);
    }
}
