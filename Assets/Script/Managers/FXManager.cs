using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    private static FXManager instance;
    public static FXManager Inst => instance;

    Dictionary<int, Stack<GameObject>> pooledFx;
    public Dictionary<int, Stack<GameObject>> PooledFx => pooledFx;

    [SerializeField] FXData[] poolingFx;
    public FXData[] PoolingFx => poolingFx;


    Stack<GameObject> bulletExplosion;
    Stack<GameObject> blankFX;

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
        blankFX = new();

        pooledFx.Add((int)FxID.BULLETEXPLOSION, bulletExplosion);
        pooledFx.Add((int)FxID.BLANKFX, blankFX);

        for (int i = 0; i < poolingFx.Length; i++)
        {
            for (int j = 0; j < poolingFx[i].poolSize; j++)
            {
                GameObject obj = Instantiate(poolingFx[i].prefab, this.transform);
                pooledFx[i].Push(obj);
                obj.SetActive(false);
            }
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
