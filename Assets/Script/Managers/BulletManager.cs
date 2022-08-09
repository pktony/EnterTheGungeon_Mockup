using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    // ##################### Singleton #########################
    private static BulletManager instance = null;
    public static BulletManager Inst { get => instance; }

    // #################### Dictionary #########################
    private static Dictionary<uint, Stack<GameObject>> pooledBullets;
    public static Dictionary<uint, Stack<GameObject>> PooledBullets => pooledBullets;

    //##################### Bullet Stack #######################
    [SerializeField] private BulletData[] poolingBullet;
        // [0] : player Bullet
        // [1] : Enemy Bullet

    private Stack<GameObject> player_Bullet;
    private Stack<GameObject> enemy_Bullet;

    private uint playerBulletID;
    private uint enemyBulletID;
    public uint PlayerBulletID => playerBulletID;
    public uint EnemyBulletID => enemyBulletID;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instance.Initialize();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void Initialize()
    {
        pooledBullets = new();
        player_Bullet = new();
        enemy_Bullet = new();

        playerBulletID = poolingBullet[0].bulletId;
        enemyBulletID = poolingBullet[1].bulletId;

        pooledBullets.Add(poolingBullet[playerBulletID].bulletId, player_Bullet);
        pooledBullets.Add(poolingBullet[enemyBulletID].bulletId, enemy_Bullet);

        for (int i = 0; i < poolingBullet.Length; i++)
        {
            for (int j = 0; j < poolingBullet[i].bulletSize; j++)
            {
                GameObject obj = Instantiate(poolingBullet[i].prefab, this.transform);
                pooledBullets[poolingBullet[i].bulletId].Push(obj);
                obj.SetActive(false);
            }
        }
    }

    //private void InitializeEnemyBullets()
    //{
    //    enemyBullets = new Queue<GameObject>();

    //    GameObject obj;
    //    for (int i = 0; i < EnemyBulletPoolNumber; i++)
    //    {
    //        obj = Instantiate(enemyBullet, this.transform);
    //        obj.SetActive(false);
    //        enemyBullets.Enqueue(obj);
    //    }
    //}

    public GameObject GetPooledBullet(Stack<GameObject> poolingObject)
    {
        if (poolingObject.Count > 0)
        {
            GameObject obj = poolingObject.Pop();
            //obj.SetActive(true);
            return obj;
        }
        return null;
    }

    public void ReturnBullet(Stack<GameObject> returningStack , GameObject uselessBullet)
    {
        returningStack.Push(uselessBullet);
        uselessBullet.SetActive(false);
    }

    //public GameObject GetEnemyBullet()
    //{
    //    if (enemyBullets.Count > 0)
    //    {
    //        GameObject obj = enemyBullets.Dequeue();
    //        obj.SetActive(true);
    //        return obj;
    //    }
    //    return null;
    //}

    //public void ReturnEnemyBullet(GameObject uselessBullet)
    //{
    //    uselessBullet.SetActive(false);
    //    enemyBullets.Enqueue(uselessBullet);
    //}
}
