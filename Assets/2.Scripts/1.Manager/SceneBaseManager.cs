using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DefineHelper;
public abstract class SceneBaseManager : MonoBehaviour
{

#if UNITY_EDITOR
    void Awake()
    {
        SoundManager._instance.Initialize(eSceneName.Start);
        SceneControlManager._instance.LoadScene((eSceneName)System.Enum.Parse(typeof(eSceneName), SceneManager.GetActiveScene().name));
    }
#endif
    public abstract void ProgInit();
    public abstract void UpdateFrame();
}
