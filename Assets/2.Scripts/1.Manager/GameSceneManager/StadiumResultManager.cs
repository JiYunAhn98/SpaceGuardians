using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StadiumResultManager : SceneBaseManager
{
    [SerializeField] TextMeshProUGUI[] _sceneText;
    [SerializeField] TextMeshProUGUI[] _resultText;
    [SerializeField] TextMeshProUGUI _totalText;
    [SerializeField] GameObject _exitBtn;
    PlayerSpawnPoint _playerSpawnPoint;   // 플레이어 생성 포인트
    float _startTime;
    int _nowIndex;
    float _score;

    public override void ProgInit()
    {
        _exitBtn.SetActive(false);
        _playerSpawnPoint = GameObject.FindWithTag("Respawn").GetComponent<PlayerSpawnPoint>();
        _playerSpawnPoint.GeneratePlayer().Initialize(1, 0, PlayerActive.PlayerFall._instance, 0, true, false);
        _startTime = Time.time + 2;
        _nowIndex = 0;
    }
    public override void UpdateFrame()
    {
        if(Time.time >= _startTime)
        {
            if (_nowIndex == _resultText.Length)
            {
                SoundManager._instance.PlayEffect(DefineHelper.eEffectSound.TotalResult);
                _totalText.text = _score.ToString();
                _exitBtn.SetActive(true);
                _nowIndex++;
            }
            else if (_nowIndex < _resultText.Length)
            {
                SoundManager._instance.PlayEffect(DefineHelper.eEffectSound.RoundResult);
                _startTime += 1;
                _resultText[_nowIndex].text = SceneControlManager._instance._resultScoreArray[_nowIndex].ToString();
                _sceneText[_nowIndex].text = SceneControlManager._instance._pickStages[_nowIndex].ToString();
                _score += SceneControlManager._instance._resultScoreArray[_nowIndex];
                _nowIndex++;
            }
        }
    }
}
