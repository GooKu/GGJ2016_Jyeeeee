﻿using UnityEngine;

/**
 *Brief: Mouse event control.
 *Author: GooKu, 
 *Revsion:
	*20151107:Frist version, increase function for handle script on gameobject.
*/

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
		private static T _instance;
		private static object _lock = new object();
		public static T Instance
		{
			get
			{
				if (applicationIsQuitting)
				{
					Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
					"' already destroyed on application quit." +
					" Won't create again - returning null.");
					return null;
				}
				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = (T)FindObjectOfType(typeof(T));
						if (FindObjectsOfType(typeof(T)).Length > 1)
						{
							Debug.LogError("[Singleton] Something went really wrong " +
							" - there should never be more than 1 singleton!" +
							" Reopenning the scene might fix it.");
							return _instance;
						}
						if (_instance == null)
						{
							GameObject singleton = new GameObject();
							_instance = singleton.AddComponent<T>();
							singleton.name = "(singleton)" + typeof(T).ToString();
							DontDestroyOnLoad(singleton);
							Debug.Log("[Singleton] An instance of " + typeof(T) +
							" is needed in the scene, so '" + singleton +
							"' was created with DontDestroyOnLoad.");
						}
						else
						{
							//Debug.Log("[Singleton] Using instance already created: " + _instance.gameObject.name);
						}
					}
					return _instance;
				}
			}
		}

	private static bool applicationIsQuitting = false;

	protected virtual void Awake() {
		if (_instance == null)
		{
			_instance = GetComponent<T>();
        }
		else {
			DestroyImmediate(gameObject);
			return;
		}
	}

	public void OnDestroy()
	{
		if (_instance==this) 
			applicationIsQuitting = true;
	}
}
