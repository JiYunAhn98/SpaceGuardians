using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelGage : MonoBehaviour
{
    [Header("Level Icon")]
    [SerializeField] Sprite[] _alarmPrefabs;
    [SerializeField] Color[] _gageColors;
    [SerializeField] SpriteRenderer _background;
    // 참조 변수
    [SerializeField] GameObject _gageColor;
    Slider _levelGage;
    Image _alarmImg;

    // 정보 변수
    int _alarmIndex;
    bool _isRedLightUp;

    public bool _gageLevelUpgrade { get { return _alarmIndex == _gageColors.Length; } }

    void Awake()
    {
        // 참조
        _levelGage = GetComponentInChildren<Slider>();
        _alarmImg = GetComponentInChildren<Image>();

        // 초기 설정
        _alarmIndex = 0;
        _isRedLightUp = false;
    }
    public void SetGage(float nowTime, float spawnTime)
    {
        float level = nowTime / spawnTime;
        if (_alarmPrefabs.Length <= (int)level)
        {
            if (_background.color.g <= 0.5)
                _isRedLightUp = true;
            else if (_background.color.g >= 1)
                _isRedLightUp = false;

            _background.color += new Color(0, 1, 1, 0) * (_isRedLightUp ? Time.deltaTime : -Time.deltaTime);

        }
        else if (_alarmIndex < (int)level)
        {
            _alarmIndex++;
            SoundManager._instance.PlayEffect(DefineHelper.eEffectSound.GageAlarm);
            if (_alarmIndex == _gageColors.Length) return;
            _alarmImg.sprite = _alarmPrefabs[_alarmIndex];
            _gageColor.GetComponent<Image>().color = _gageColors[_alarmIndex];
            _levelGage.value = 0;
        }
        else
        {
            _levelGage.value = level - _alarmIndex;
        }
    }
}
