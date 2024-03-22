using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	//----------------------------------------
	static T _uniqueInstance = null;
	static GameObject _uniqueObject = null;
	//----------------------------------------
	public static T _instance
	{
		get
		{
			if (_uniqueInstance == null)
			{
				lock (typeof(T))
				{
                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("--- FSMSingleton error ---");
                        return _instance;
                    }
                    else
                    {
                        _uniqueInstance = (T)FindObjectOfType(typeof(T));
                    }

                    if (_uniqueInstance == null && _uniqueObject == null)
					{
						_uniqueObject = new GameObject(typeof(T).Name);
						_uniqueInstance = _uniqueObject.AddComponent<T>();
					}
					else
						Debug.LogError("--- FSMSingleton already exists ---");

				}
			}
			return _uniqueInstance;
		}
	}
}
