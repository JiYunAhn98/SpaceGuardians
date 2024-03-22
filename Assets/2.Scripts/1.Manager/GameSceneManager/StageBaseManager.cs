using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

// �������̽��� ��������
// �������������� �и�
// Ư�� ����� �ִٸ� �װ� ��󿡰Ը� �ְ� ������ ������� 
// �߻�Ŭ������ ���, ��ɸ�����ϴ� Ŭ���� 
// �������̽��� ����ó�� ������ �����ִ� �߰��ٸ� ����
// Ŭ������ ������ ���� �η������� ����
// Ŭ������ �ڽ��� ����� �����ٸ� �װɷ� ���̴�.


public abstract class StageBaseManager : SceneBaseManager
{
    protected eGameState _nowGameState;

    // UI
    [SerializeField] protected TimerBox _timer;                      // �ð��� �����ִ� Ÿ�̸�
    protected AlarmBox _msgBox;                     // ���� ���¸� �˷��ִ� �˸�â

    public string _nowtime { get { return _timer.GetTime(); } }

    // Player
    protected PlayerSpawnPoint _playerSpawnPoint;   // �÷��̾� ���� ����Ʈ
    protected Player _player;                       // ���ӿ��� ������ ������Ʈ
    protected float _playTime;                      // ���� ����� �ð�

    public abstract IEnumerator ProgReady();
    public abstract void ProgPlay();
    public abstract void ProgEnd(bool isSuceess);
    public abstract IEnumerator ProgResult();
}
