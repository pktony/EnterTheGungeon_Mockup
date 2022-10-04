using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : Singleton<FXManager>
{
    Dictionary<int, Stack<GameObject>> pooledFx;
    public Dictionary<int, Stack<GameObject>> PooledFx => pooledFx;

    [SerializeField] FXData[] poolingFx;
    public FXData[] PoolingFx => poolingFx;
        
    Stack<GameObject> blankFX;


    protected override void Initialize()
    {
        pooledFx = new();
        blankFX = new();

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
