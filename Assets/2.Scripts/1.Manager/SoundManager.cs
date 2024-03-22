using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoSingleton<SoundManager>
{
    // resource 참조
    AudioClip[] _effectAudioSources;
    AudioClip[] _bgmAudioSources;

    // 참조 변수
    AudioSource _bgmSoundPlayer;
    AudioSource _effectSoundPlayer;
    public bool _isMusicStop { get; set; }
    public float _soundVolume { get { return _bgmSoundPlayer.volume; } }

    int BGMCount(eSceneName scene)
    {
        return (int)scene - (int)eSceneName.Start;
    }

    // 초기설정
    public void Initialize(eSceneName scene)
    {
        if (_effectSoundPlayer != null || _bgmSoundPlayer != null) return;

        _effectSoundPlayer = new GameObject("EffectSoundPlayer").AddComponent<AudioSource>();
        _effectSoundPlayer.transform.parent = gameObject.transform;
        _bgmSoundPlayer = new GameObject("BGMSoundPlayer").AddComponent<AudioSource>();
        _bgmSoundPlayer.transform.parent = gameObject.transform;

        _effectAudioSources = new AudioClip[(int)eEffectSound.Count];
        for (int i = 0; i < (int)eEffectSound.Count; i++)
        {
            _effectAudioSources[i] = Resources.Load("Sound/Effect/" + ((eEffectSound)i).ToString()) as AudioClip;
        }
        _bgmAudioSources = new AudioClip[BGMCount(eSceneName.Count)];
        for (int i = (int)eSceneName.Start; i < (int)eSceneName.Count; i++)
        {
            _bgmAudioSources[i - (int)eSceneName.Start] = Resources.Load("Sound/BGM/" + ((eSceneName)i).ToString()) as AudioClip;
        }
        _isMusicStop = false;
        PlayBGM(scene);
    }

    // 소리를 Play
    public void PlayBGM(eSceneName type, bool _isLoop = true)
    {
        if (_isMusicStop) return;

        _bgmSoundPlayer.clip = _bgmAudioSources[BGMCount(type)];
        _bgmSoundPlayer.loop = _isLoop;

        _bgmSoundPlayer.Play();
    }
    public void PlayEffect(eEffectSound type)
    {
        if (_isMusicStop) return;

        _effectSoundPlayer.PlayOneShot(_effectAudioSources[(int)type]);
    }
    public void BGMSoundDown(float sound)
    {
        if (_isMusicStop) return;

        if (_bgmSoundPlayer.volume > 0)
        {
            _bgmSoundPlayer.volume -= sound;
        }
    }
    public void BGMSoundUp(float sound)
    {
        if (_isMusicStop) return;

        if (_bgmSoundPlayer.volume < 1)
        {
            _bgmSoundPlayer.volume += sound;
        }
    }
}
