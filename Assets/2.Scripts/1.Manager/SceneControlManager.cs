using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
using UnityEngine.SceneManagement;

public class SceneControlManager : MonoSingleton<SceneControlManager>
{
    // Loading Bar Prefab
    GameObject _prefabLoadingCanvas;

    // 정보 변수
    AsyncOperation _aOper;
    SceneLoadingCanvas _loadingCanvas;
    SceneBaseManager _scenemanager;

    eSceneName[] _selectStages = new eSceneName[3];
    int[] _score;
    int _pickStageIndex;

    public eSceneName[] _pickStages { get { return _selectStages; } }
    public int _resultScore { get { return _score[_pickStageIndex]; } set { _score[_pickStageIndex] = value; } }
    public int[] _resultScoreArray { get { return _score; } }
    public bool _loadEnd { get { return !_loadingCanvas.gameObject.activeSelf; } }
    public eSceneName _currentScene { get; set; }
    public eSceneName _returnScene { get; set; }

    void Update()
    {
        if (_scenemanager != null && _loadEnd)
        {
            _scenemanager.UpdateFrame();
        }
    }

    public void LoadStadiumNextScene()
    {
        if (_pickStageIndex < 2)
        {
            if(_pickStageIndex == -1) SelectGames();
            LoadScene(_selectStages[++_pickStageIndex]);
        } 
        else
        {
            _pickStageIndex = -1;
            LoadScene(eSceneName.StadiumResult);
        }
    }
    public void LoadReturnScene()
    {
        LoadScene(_returnScene);
    }
    public void LoadScene(eSceneName scene)
    {
        if (_loadingCanvas == null)
        {
            _prefabLoadingCanvas = Resources.Load("UI/SceneLoadingCanvas") as GameObject;
            GameObject go = Instantiate(_prefabLoadingCanvas, transform);
            _loadingCanvas = go.GetComponent<SceneLoadingCanvas>();
        }
        else
        {
            if (!_loadEnd)
                return;
            _loadingCanvas.gameObject.SetActive(true);
        }
        StopAllCoroutines();
        _scenemanager = null;

        switch (scene)
        {
            case eSceneName.Galaxy:
            case eSceneName.Stadium:
            case eSceneName.SpaceShip:
                _returnScene = eSceneName.Lobby;
                _pickStageIndex = -1;
                break;
            default:
                if (_returnScene == eSceneName.SpaceShip || _returnScene == eSceneName.Stadium)
                    break;
                else if (_currentScene == eSceneName.SpaceShip && scene != eSceneName.Lobby)
                    _returnScene = eSceneName.SpaceShip;
                else if (_currentScene == eSceneName.Stadium && scene != eSceneName.Lobby)
                    _returnScene = eSceneName.Stadium;
                else
                    _returnScene = eSceneName.Lobby;
                break;
        }
        _currentScene = scene;

        _loadingCanvas.OpenLoadingWindow();

    }
    public IEnumerator LoadStart()
    {
        _loadingCanvas.OnLoadingAnimation();
        yield return new WaitForSeconds(1.0f);
        _aOper = SceneManager.LoadSceneAsync(_currentScene.ToString());
        UIManager._instance.CloseAllPopup();


        while (!_aOper.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        _loadingCanvas.OffLoadingAnimation();
        _scenemanager = GameObject.FindGameObjectWithTag("Manager").GetComponent<SceneBaseManager>();
        _scenemanager.ProgInit();

        GameObject[] pcUIs = GameObject.FindGameObjectsWithTag("PCUI");

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            foreach (GameObject pcUI in pcUIs)
            {
                pcUI.SetActive(true);
            }
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            foreach (GameObject pcUI in pcUIs)
            {
                pcUI.SetActive(false);
            }
        }

        StartCoroutine(_loadingCanvas.FadeIn());
    }
    void SelectGames()
    {
        _selectStages = new eSceneName[3];
        _score = new int[3];
        for (int i = 0; i < _selectStages.Length; i++)
        {
            eSceneName selectStage = ((eSceneName)Random.Range(0, (int)eSceneName.Count));
            _selectStages[i] = selectStage;
            for (int tmp = 0; tmp < i; tmp++)
            {
                if (_selectStages[tmp] == selectStage)
                {
                    i--;
                    break;
                }
            }
        }
    }
}
