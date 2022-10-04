using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance = null;
    public static T Inst
    {
        get
        {
            if (instance == null)
            {
                T obj = FindObjectOfType<T>();
                if(obj != null)
                {
                    instance = obj;
                }
                else
                {
                    GameObject gameObject = new();
                    gameObject.name = $"{typeof(T).Name}";
                    instance = gameObject.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            T obj = this as T;
            DontDestroyOnLoad(obj.gameObject);
            SceneManager.sceneLoaded += OnSceneLoad;
        }
        else
        {
            if(instance != this)
            {
                Destroy(this.gameObject );
            }
        }
    }

    protected virtual void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
    {
    }

    protected virtual void Initialize()
    { }

}
