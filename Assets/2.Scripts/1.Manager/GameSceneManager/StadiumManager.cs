using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DefineHelper;
using PlayerActive;

public class StadiumManager : SceneBaseManager
{
    static StadiumManager _uniqueInstance;

    // Resources
    Sprite[] _planets;
    Sprite[] _backgrounds;

    // 참조 변수
    SpriteRenderer _spritePlanet;
    SpriteRenderer _spriteBackground;
    PlayerSpawnPoint _playerSpawnPoint;
    Transform _characterSlotStartPos;
    List<CharacterPickingSlot> _characterSlots;

    //UI 참조 변수
    TimerBox _timer;
    StartButton _startBtn;

    // 정보 변수
    bool _countStart;
    float _time;
    int _alarmTime;
    Player _player;
    Dictionary<eCharacterName, Player> _genCharacters;

    public static StadiumManager _instance { get { return _uniqueInstance; } }
    public bool _isCountStart { get { return _countStart; } }

    public override void ProgInit()
    {
        _uniqueInstance = this;

        // 초기 참조
        _timer = GameObject.FindGameObjectWithTag("UITimer").GetComponent<TimerBox>();
        _startBtn = GameObject.FindGameObjectWithTag("UIStartBtn").GetComponent<StartButton>();
        _spritePlanet = GameObject.FindGameObjectWithTag("Planet").GetComponent<SpriteRenderer>();
        _spriteBackground = GameObject.Find("Background").GetComponent<SpriteRenderer>();
        _playerSpawnPoint = GameObject.FindGameObjectWithTag("Respawn").GetComponent<PlayerSpawnPoint>();
        _characterSlotStartPos = GameObject.FindGameObjectWithTag("UISlotSpawnPos").GetComponent<Transform>();
        _characterSlots = new List<CharacterPickingSlot>();
        _genCharacters = new Dictionary<eCharacterName, Player>();
        
        // 리소스 불러오기
        GameObject characterSlotPrefab = Resources.Load("UI/CharacterPickingSlot") as GameObject;
        for (int i = 0; i < (int)eCharacterName.Count; i++)
        {
            GameObject go = Instantiate(characterSlotPrefab, _characterSlotStartPos);
            go.GetComponent<RectTransform>().anchoredPosition+= new Vector2(i * 250, 0);
            _characterSlots.Add(go.GetComponent<CharacterPickingSlot>());
            _characterSlots[i].SetIcon((eCharacterName)i);
        }
        _planets = new Sprite[(int)ePlanetName.Count];
        _backgrounds = new Sprite[(int)ePlanetName.Count];
        for (int i = 0; i < _planets.Length; i++)
        {
            _planets[i] = Resources.Load<Sprite>("Planet/" + ((ePlanetName)i).ToString());
            _backgrounds[i] = Resources.Load<Sprite>("Background/" + ((ePlanetName)i).ToString());
        }


        // 초기 설정
        _countStart = false;
        _time = 0;
        _timer.CloseTimerBox();
        ChangeCharacter(PlayerManager._instance._nowPickCharacter);
    }

    public override void UpdateFrame()
    {
        if (_countStart)
        {
            _time -= Time.deltaTime;
            _spritePlanet.transform.Rotate(new Vector3(0, 0, -0.1f));
            _spriteBackground.size += Vector2.right * Time.deltaTime;
            _timer.SetTime(_time);

            if (_time <= _alarmTime)
            {
                SoundManager._instance.PlayEffect(eEffectSound.AlarmTime);
                _alarmTime -= 1;
            }

            if (_time <= 1)
            {
                _countStart = false;
                SceneControlManager._instance.LoadStadiumNextScene();
            }
        }
    }
    public void ClickforStartOrStop()
    {
        if (_countStart)
        {
            SoundManager._instance.PlayEffect(eEffectSound.TimerStop);
            _startBtn.isStop();
            _timer.CloseTimerBox();
        }
        else
        {
            _time = 6;
            _alarmTime = 5;
            SoundManager._instance.PlayEffect(eEffectSound.TimerStart);
            _startBtn.isStart();
            _timer.OpenTimerBox(_time);
        }
        _countStart = !_countStart;
    }

    /// <summary>
    ///  Character Slot을 고르면 활성화, 현재 고른 캐릭터를 바꿔준다.
    /// </summary>
    /// <param name="nowPick">현재 고른 캐릭터</param>
    public void ChangeCharacter(eCharacterName nowPick)
    {
        // 현재 캐릭터를 비활성화하고 nowPick에 해당하는 캐릭터를 활성화
        if (_player != null)
        {
            SoundManager._instance.PlayEffect(eEffectSound.CharacterPick);

            _characterSlots[(int)PlayerManager._instance._nowPickCharacter].PickOtherSlot();
            _player.gameObject.SetActive(false);
        }
        PlayerManager._instance._nowPickCharacter = nowPick;
        _characterSlots[(int)PlayerManager._instance._nowPickCharacter].PickThisSlot();



        // 이미 생성했던 캐릭터라면 활성화하고 아니면 새로 생성
        if (_genCharacters.ContainsKey(nowPick))
        {
            _player = _genCharacters[nowPick];
            _player.gameObject.SetActive(true);
        }
        else
        {
            _player = _playerSpawnPoint.GeneratePlayer();
            _genCharacters.Add(nowPick, _player);
            _player.Initialize(1, 0, PlayerIdle._instance, 0, false, false);
        }

        // 플레이어에 해당하는 행성 불러오기
        string planet = TableManager._instance.TakeString(TableManager.eTableJsonNames.CharacterInfo, (int)nowPick, TableManager.eCharacterInfoIndex.Planet.ToString());

        _spriteBackground.sprite = _backgrounds[(int)System.Enum.Parse(typeof(ePlanetName), planet)];
        _spritePlanet.sprite = _planets[(int)System.Enum.Parse(typeof(ePlanetName), planet)];
    }
}
