using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private EnemyData[] poolingEnemies;
    public EnemyData[] PoolingEnemies => poolingEnemies;
        // [0] : Shotgun Kin
        // [1] : Bullet Kin

    private Dictionary<int, Stack<GameObject>> pooledEnemy = new();
    public Dictionary<int, Stack<GameObject>> PooledEnemy => pooledEnemy;

    private Stack<GameObject> shotgunKin;
    private Stack<GameObject> bulletKin;


    protected override void Initialize()
    {
        shotgunKin = new();
        bulletKin = new();

        pooledEnemy.Add(poolingEnemies[0].EnemyId, shotgunKin);
        pooledEnemy.Add(poolingEnemies[1].EnemyId, bulletKin);

        for (int j = 0; j < poolingEnemies[0].poolSize; j++)
        {
            GameObject obj = Instantiate(poolingEnemies[0].prefab, this.transform);
            pooledEnemy[(int)EnemyID.SHOTGUNKIN].Push(obj);
            obj.SetActive(false);
        }

        for (int j = 0; j < poolingEnemies[1].poolSize; j++)
        {
            GameObject obj = Instantiate(poolingEnemies[1].prefab, this.transform);
            pooledEnemy[(int)EnemyID.BULLETKIN].Push(obj);
            obj.SetActive(false);
        }
    }

    public GameObject GetEnemy(EnemyID id)
    {
        if (PooledEnemy[(int)id].Count > 0)
        {
            GameObject enemy = pooledEnemy[(int)id].Pop();
            return enemy;
        }
        else
        {
            GameObject obj = Instantiate(poolingEnemies[(int)id].prefab, this.transform);
            pooledEnemy[(int)id].Push(obj);
            obj.SetActive(false);
            return obj;
        }
    }

    public void ReturnEnemy(Stack<GameObject> EnemyStack, GameObject uselessEnemy)
    {
        EnemyStack.Push(uselessEnemy);
        uselessEnemy.SetActive(false);
        uselessEnemy.transform.position = Vector3.zero;
        uselessEnemy.transform.rotation = Quaternion.identity;
    }

    public void ReturnEnemy(EnemyID id, GameObject uselessEnemy)
    {
        ReturnEnemy(pooledEnemy[(int)id], uselessEnemy);
    }
}
