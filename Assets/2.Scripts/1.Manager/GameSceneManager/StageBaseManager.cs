using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

// 인터페이스로 구현하자
// 행위를기준으로 분리
// 특정 기능이 있다면 그걸 대상에게만 주고 공통의 ㄱ기능은 
// 추상클래스와 고민, 기능만담당하는 클래스 
// 인터페이스는 소켓처럼 연결을 도와주는 중간다리 역할
// 클래스를 나누는 것을 두려워하지 말자
// 클래스는 자신의 기능을 가진다면 그걸로 끝이다.


public abstract class StageBaseManager : SceneBaseManager
{
    protected eGameState _nowGameState;

    // UI
    [SerializeField] protected TimerBox _timer;                      // 시간을 보여주는 타이머
    protected AlarmBox _msgBox;                     // 게임 상태를 알려주는 알림창

    public string _nowtime { get { return _timer.GetTime(); } }

    // Player
    protected PlayerSpawnPoint _playerSpawnPoint;   // 플레이어 생성 포인트
    protected Player _player;                       // 게임에서 조종할 오브젝트
    protected float _playTime;                      // 현재 진행된 시간

    public abstract IEnumerator ProgReady();
    public abstract void ProgPlay();
    public abstract void ProgEnd(bool isSuceess);
    public abstract IEnumerator ProgResult();
}
