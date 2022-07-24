using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    // ##################### Singleton #########################
    private static BulletManager bullet_instance = null;
    public static BulletManager Bullet_Inst { get => bullet_instance; }

    //##################### Bullet Queue #######################
    private Queue<GameObject> playerBullets;
    private Queue<GameObject> enemyBullets;
    public GameObject playerBullet;
    public GameObject enemyBullet;

    private uint PlayerBulletPoolNumber = 30;
    private uint EnemyBulletPoolNumber = 100;

    // #################### Player Bullet #########################

    private void Awake()
    {
        if (bullet_instance == null)
        {
            bullet_instance = this;
            bullet_instance.Initialize();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (bullet_instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void Initialize()
    {
        InitializePlayerBullets();
        InitializeEnemyBullets();
    }

    private void InitializePlayerBullets()
    {
        playerBullets = new Queue<GameObject>();

        GameObject obj;
        for (int i = 0; i < PlayerBulletPoolNumber; i++)
        {
            obj = Instantiate(playerBullet, this.transform);
            obj.SetActive(false);
            playerBullets.Enqueue(obj);
        }
    }
    private void InitializeEnemyBullets()
    {
        enemyBullets = new Queue<GameObject>();

        GameObject obj;
        for (int i = 0; i < EnemyBulletPoolNumber; i++)
        {
            obj = Instantiate(enemyBullet, this.transform);
            obj.SetActive(false);
            enemyBullets.Enqueue(obj);
        }
    }

    public GameObject GetPlayerBullet()
    {
        if (playerBullets.Count > 0)
        {
            GameObject obj = playerBullets.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return null;
    }

    public void ReturnPlayerBullet(GameObject uselessBullet)
    {
        uselessBullet.SetActive(false);
        playerBullets.Enqueue(uselessBullet);
    }

    public GameObject GetEnemyBullet()
    {
        if (enemyBullets.Count > 0)
        {
            GameObject obj = enemyBullets.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return null;
    }

    public void ReturnEnemyBullet(GameObject uselessBullet)
    {
        uselessBullet.SetActive(false);
        enemyBullets.Enqueue(uselessBullet);
    }
}
