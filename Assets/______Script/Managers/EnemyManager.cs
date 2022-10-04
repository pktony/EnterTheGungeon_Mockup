using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private EnemyData[] poolingEnemies;
    public EnemyData[] PoolingEnemies => poolingEnemies;
        // [0] : Shotgun Kin
        // [1] : Bullet Kin

    private Dictionary<EnemyID, Stack<GameObject>> pooledEnemy = new();
    public Dictionary<EnemyID, Stack<GameObject>> PooledEnemy => pooledEnemy;

    private Stack<GameObject> shotgunKin;
    private Stack<GameObject> bulletKin;

    protected override void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
    {
        if (pooledEnemy.Count < 1)
        {
            shotgunKin = new();
            bulletKin = new();

            pooledEnemy.Add(EnemyID.SHOTGUNKIN, shotgunKin);
            pooledEnemy.Add(EnemyID.BULLETKIN, bulletKin);

            for (int j = 0; j < poolingEnemies[0].poolSize; j++)
            {
                GameObject obj = Instantiate(poolingEnemies[0].prefab, this.transform);
                pooledEnemy[(int)EnemyID.SHOTGUNKIN].Push(obj);
                obj.SetActive(false);
            }

            for (int j = 0; j < poolingEnemies[1].poolSize; j++)
            {
                GameObject obj = Instantiate(poolingEnemies[1].prefab, this.transform);
                pooledEnemy[EnemyID.BULLETKIN].Push(obj);
                obj.SetActive(false);
            }
        }
    }

    public GameObject GetEnemy(EnemyID id)
    {
        if (PooledEnemy[id].Count > 0)
        {
            GameObject enemy = pooledEnemy[id].Pop();
            return enemy;
        }
        else
        {
            GameObject obj = Instantiate(poolingEnemies[(int)id].prefab, this.transform);
            pooledEnemy[id].Push(obj);
            obj.SetActive(false);
            return obj;
        }
    }

    public void ReturnEnemy(Stack<GameObject> EnemyStack, GameObject uselessEnemy)
    {
        EnemyStack.Push(uselessEnemy);
        uselessEnemy.SetActive(false);
        uselessEnemy.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public void ReturnEnemy(EnemyID id, GameObject uselessEnemy)
    {
        ReturnEnemy(pooledEnemy[id], uselessEnemy);
    }

    public void ReturnAllEnemies()
    {
        BulletKin[] bulletkins = FindObjectsOfType<BulletKin>(false);
        ShotgunKin[] shotgunKins = FindObjectsOfType<ShotgunKin>(false);

        for (int i = 0; i < bulletkins.Length; i++)
            ReturnEnemy(EnemyID.BULLETKIN, bulletkins[i].gameObject);
        for (int i = 0; i < shotgunKins.Length; i++)
            ReturnEnemy(EnemyID.SHOTGUNKIN, shotgunKins[i].gameObject);
    }
}
