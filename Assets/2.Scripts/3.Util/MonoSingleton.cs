using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    static volatile T _uniqueInstance = null;
    static volatile GameObject _uniqueObject = null;

    protected MonoSingleton()
    {
    }

    public static T _instance
    {
        get
        {
            if (_uniqueInstance == null)
            {
                lock (typeof(T))
                {
                    if (_uniqueInstance == null && _uniqueObject == null)
                    {
                        _uniqueObject = new GameObject(typeof(T).Name, typeof(T));
                        _uniqueInstance = _uniqueObject.GetComponent<T>();
                        _uniqueInstance.Init();
                    }
                }
            }
            return _uniqueInstance;
        }
    }

    public virtual void Init()
    {
        DontDestroyOnLoad(this);
    }
}
