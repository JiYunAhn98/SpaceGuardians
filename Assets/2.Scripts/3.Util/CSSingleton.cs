using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSingleton<T> where T : CSSingleton<T>
{
    static volatile T _uniqueInstance;

    protected CSSingleton()
    {
    }

    public static T _instance
    {
        get 
        {
            if (_uniqueInstance == null)
            {
                _uniqueInstance = System.Activator.CreateInstance(typeof(T)) as T;
            }
            return _uniqueInstance;
        }
    }
}
