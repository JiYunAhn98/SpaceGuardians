using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DefineHelper;
using PlayerActive;

public class GalaxyManager : SceneBaseManager
{
    static GalaxyManager _uniqueInstance;
    // Resources
    GameObject[] _prefabCharacters;
    Sprite[] _prefabPlanets;

    // 참조 변수
    TextMeshProUGUI _leftText;
    TextMeshProUGUI _rightText;
    PlayerSpawnPoint _playerSpawnPoint;
    SpriteRenderer _planet;

    // 정보 변수
    public override void UpdateFrame()
    {
    }
    // 오브젝트 변수
    Dictionary<eCharacterName, Player> _characters;

    public static GalaxyManager _instance { get { return _uniqueInstance; } }
    public override void ProgInit()
    {
        _uniqueInstance = this;
        _playerSpawnPoint = GameObject.FindWithTag("Respawn").GetComponent<PlayerSpawnPoint>();
        _characters = new Dictionary<eCharacterName, Player>();
        _leftText = GameObject.Find("LeftText").GetComponentInChildren<TextMeshProUGUI>();
        _rightText = GameObject.Find("RightText").GetComponentInChildren<TextMeshProUGUI>();
        _planet = GameObject.Find("Planet").GetComponentInChildren<SpriteRenderer>();

        _prefabCharacters = new GameObject[(int)eCharacterName.Count];
        for (int i = 0; i < _prefabCharacters.Length; i++)
        {
            _prefabCharacters[i] = Resources.Load("PlayerCharacter/" + ((eCharacterName)i).ToString()) as GameObject;
        }

        _prefabPlanets = new Sprite[(int)ePlanetName.Count];
        for (int i = 0; i < _prefabPlanets.Length; i++)
        {
            _prefabPlanets[i] = Resources.Load<Sprite>("Planet/" + ((ePlanetName)i).ToString());
        }

        SpawnPlayer(PlayerManager._instance._nowPickCharacter);
    }
    public void SpawnPlayer(eCharacterName spawnObj)
    {
        SoundManager._instance.PlayEffect(eEffectSound.SwingPlanet);

        if (_characters.ContainsKey(PlayerManager._instance._nowPickCharacter))
        {
            _characters[PlayerManager._instance._nowPickCharacter].gameObject.SetActive(false);
        }

        PlayerManager._instance._nowPickCharacter = spawnObj;
        if (_characters.ContainsKey(spawnObj))
        {
            _characters[spawnObj].gameObject.SetActive(true);
            _characters[spawnObj].ChangeState(PlayerDoubleJump._instance);
        }
        else
        {
            _characters.Add(spawnObj, _playerSpawnPoint.GeneratePlayer(spawnObj));
            _characters[spawnObj].Initialize(1, 0, PlayerDoubleJump._instance, 10, true);
        }

        _leftText.text = "";
        _leftText.text += "이름: " + TableManager._instance.TakeString(TableManager.eTableJsonNames.CharacterInfo, (int)spawnObj, TableManager.eCharacterInfoIndex.Character.ToString()) + "\n";
        _leftText.text += "행성: " + TableManager._instance.TakeString(TableManager.eTableJsonNames.CharacterInfo, (int)spawnObj, TableManager.eCharacterInfoIndex.Planet.ToString()) + "\n";
        _leftText.text += "타입: " + TableManager._instance.TakeString(TableManager.eTableJsonNames.CharacterInfo, (int)spawnObj, TableManager.eCharacterInfoIndex.Type.ToString()) + "\n";
        _leftText.text += "무기: " + TableManager._instance.TakeString(TableManager.eTableJsonNames.CharacterInfo, (int)spawnObj, TableManager.eCharacterInfoIndex.Weapon.ToString()) + "\n";
        _leftText.text += TableManager._instance.TakeString(TableManager.eTableJsonNames.CharacterInfo, (int)spawnObj, TableManager.eCharacterInfoIndex.WeaponText.ToString());

        _rightText.text = TableManager._instance.TakeString(TableManager.eTableJsonNames.CharacterInfo, (int)spawnObj, TableManager.eCharacterInfoIndex.CharacterText.ToString());
        string planet = TableManager._instance.TakeString(TableManager.eTableJsonNames.CharacterInfo, (int)spawnObj, TableManager.eCharacterInfoIndex.Planet.ToString());
        _planet.sprite = _prefabPlanets[(int)System.Enum.Parse(typeof(ePlanetName), planet)];

    }
}
