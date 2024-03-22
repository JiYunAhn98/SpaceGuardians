using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DefineHelper;
public class SceneLoadingCanvas : MonoBehaviour
{
    [SerializeField] RectTransform _stadiumGameBox;
    [SerializeField] RectTransform _spaceShipGameBox;
    [SerializeField] Image _blackFade;

    [Header("Loading Animation")]
    [SerializeField] GameObject _loadingMove;
    [SerializeField] TextMeshProUGUI _loadingText;

    //정보변수
    Vector2 _originBoxPos;
    CanvasScaler _resolution;


    #region[외부함수]
    public void OpenLoadingWindow()
    {
        if (_originBoxPos == Vector2.zero)
        {
            _resolution = gameObject.GetComponent<CanvasScaler>();
            _resolution.referenceResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
            _stadiumGameBox.gameObject.GetComponent<RectTransform>().sizeDelta = _resolution.referenceResolution;
            _spaceShipGameBox.gameObject.GetComponent<RectTransform>().sizeDelta = _resolution.referenceResolution;

            _stadiumGameBox.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Screen.currentResolution.height);
            _spaceShipGameBox.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Screen.currentResolution.height);
        }

        gameObject.SetActive(true);
        StartCoroutine("FadeOut");
    }

#region[Fade In & Out]
    public IEnumerator FadeOut()
    {
        Color color = _blackFade.color;

        while (color.a < 1.0f)
        {
            color.a += Time.deltaTime;
            _blackFade.color = color;
            SoundManager._instance.BGMSoundDown(Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        switch (SceneControlManager._instance._returnScene)
        {
            case eSceneName.Stadium:
                yield return StartCoroutine(StadiumBoxDown());
                break;
            case eSceneName.SpaceShip:
                yield return StartCoroutine(SpaceShipBoxDown());
                break;
        }

        StartCoroutine(SceneControlManager._instance.LoadStart());
    }
    public IEnumerator FadeIn()
    {
        SoundManager._instance.PlayBGM(SceneControlManager._instance._currentScene);
        switch (SceneControlManager._instance._returnScene)
        {
            case eSceneName.Stadium:
                yield return StartCoroutine(StadiumBoxUp());
                break;
            case eSceneName.SpaceShip:
                yield return StartCoroutine(SpaceShipBoxUp());
                break;
        }
        Color color = _blackFade.color;

        while (color.a > 0)
        {
            color.a -= Time.deltaTime;
            SoundManager._instance.BGMSoundUp(Time.deltaTime);
            _blackFade.color = color;
            yield return new WaitForEndOfFrame();
        }
        gameObject.SetActive(false);
        yield break;
    }
#endregion[Fade In & Out]

#region[StadiumGame]
    public IEnumerator StadiumBoxDown()
    {
        SoundManager._instance.PlayEffect(eEffectSound.BoxDown);
        _stadiumGameBox.gameObject.SetActive(true);
        _stadiumGameBox.gameObject.GetComponent<StadiumGameBox>().NowGameAlarm();
        _originBoxPos = _stadiumGameBox.anchoredPosition;

        while (_stadiumGameBox.position.y >= 0)
        {
            _stadiumGameBox.anchoredPosition += _originBoxPos.y * Vector2.down * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }
    public IEnumerator StadiumBoxUp()
    {
        SoundManager._instance.PlayEffect(eEffectSound.BoxUp);
        while (_stadiumGameBox.anchoredPosition.y <= _originBoxPos.y)
        {
            _stadiumGameBox.anchoredPosition += _originBoxPos.y * Vector2.up * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _stadiumGameBox.gameObject.SetActive(false);
        yield break;
    }
#endregion[StadiumGame]

#region[SpaceShipGame]
    public IEnumerator SpaceShipBoxDown()
    {
        SoundManager._instance.PlayEffect(eEffectSound.BoxDown);
        _spaceShipGameBox.gameObject.SetActive(true);
        _spaceShipGameBox.gameObject.GetComponent<SpaceShipGameBox>().SelectGame();
        _originBoxPos = _spaceShipGameBox.anchoredPosition;

        while (_spaceShipGameBox.position.y >= 0)
        {
            _spaceShipGameBox.anchoredPosition += _originBoxPos.y * Vector2.down * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield break;
    }
    public IEnumerator SpaceShipBoxUp()
    {
        SoundManager._instance.PlayEffect(eEffectSound.BoxUp);
        while (_spaceShipGameBox.anchoredPosition.y <= _originBoxPos.y)
        {
            _spaceShipGameBox.anchoredPosition += _originBoxPos.y * Vector2.up * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _spaceShipGameBox.gameObject.SetActive(false);
        yield break;
    }
#endregion[SpaceShipGame]

#endregion[외부함수]

#region[내부함수]
    public void OnLoadingAnimation()
    {
        _loadingMove.SetActive(true);
    }
    public void OffLoadingAnimation()
    {
        _loadingMove.SetActive(false);
    }
#endregion[내부함수]
}
