using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    private static FXManager instance;
    public static FXManager Inst => instance;

    Dictionary<int, Stack<GameObject>> pooledFx;
    public Dictionary<int, Stack<GameObject>> PooledFx;

    [SerializeField] FXData[] poolingFx;
    public FXData[] PoolingFx => poolingFx;


    Stack<GameObject> bulletExplosion;
    public Stack<GameObject> BulletExplosion;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instance.Initialize();
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Initialize()
    {
        pooledFx = new();
        bulletExplosion = new();

        pooledFx.Add((int)FxID.BULLETEXPLOSION, bulletExplosion);

        for (int i = 0; i < poolingFx[(int)FxID.BULLETEXPLOSION].poolSize; i++)
        {
            GameObject obj = Instantiate(poolingFx[(int)FxID.BULLETEXPLOSION].prefab, this.transform);
            pooledFx[(int)FxID.BULLETEXPLOSION].Push(obj);
            obj.SetActive(false);   
        }
    }

    public GameObject GetFX(Stack<GameObject> poolingFX)
    {
        if (poolingFX.Count > 0)
        {
            GameObject obj = poolingFX.Pop();
            return obj;
        }
        return null;
    }

    public void ReturnFX(Stack<GameObject> returningStack, GameObject uselessFX)
    {
        returningStack.Push(uselessFX);
        uselessFX.transform.rotation = Quaternion.identity;
        uselessFX.transform.position = Vector3.zero;
        uselessFX.SetActive(false);
    }
}
