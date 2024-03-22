using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.GetChild(0).gameObject.activeSelf)
        {
            SoundManager._instance.PlayEffect(DefineHelper.eEffectSound.WindowClose);
        }
        else
        {
            SoundManager._instance.PlayEffect(DefineHelper.eEffectSound.WindowOpen);
        }
        transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE || UNITY_ANDROID
        Application.Quit();
#endif    
    }
    public void MusicOff()
    {
        SoundManager._instance.PlayEffect(DefineHelper.eEffectSound.MusicOff);
        SoundManager._instance.BGMSoundDown(0);
        SoundManager._instance._isMusicStop = true;
    }
    public void MusicOn()
    {
        SoundManager._instance._isMusicStop = false;
        SoundManager._instance.BGMSoundUp(1);
        SoundManager._instance.PlayEffect(DefineHelper.eEffectSound.MusicOn);
    }
}
