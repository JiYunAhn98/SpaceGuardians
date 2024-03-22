using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class StartSceneManager : MonoBehaviour
{
    float _time;
    void Awake()
    {
#if UNITY_ANDROID
        Screen.SetResolution(2960, 1440, true);
        Application.targetFrameRate = 60;
#else
        Screen.SetResolution(1920, 1080, true);
        QualitySettings.vSyncCount = 1;
#endif
        SoundManager._instance.Initialize(eSceneName.Start);
        
        _time = 0;
    }
    void Update()
    {
        _time += Time.deltaTime;
        if (_time >= 3 && (Input.anyKeyDown || Input.touchCount > 0))
        {
            SceneControlManager._instance.LoadScene(eSceneName.Lobby);
        }
    }
}
