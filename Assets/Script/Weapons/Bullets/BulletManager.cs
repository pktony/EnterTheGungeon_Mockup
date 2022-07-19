using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    // ##################### Singleton #########################
    private static BulletManager instance = null;
    public static BulletManager Inst { get; }

    //##################### Bullet Queue #######################
    private Queue<GameObject> playerBullets;
    public GameObject playerBullet;

    private uint BulletPoolNumber = 30;

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

    }

    private void Start()
    {
        playerBullets = new Queue<GameObject>();

        GameObject obj;
        for (int i = 0; i < BulletPoolNumber; i++)
        {
            obj = Instantiate(playerBullet, this.transform);
            obj.SetActive(false);
            playerBullets.Enqueue(obj);
        }
    }


    public GameObject GetPooledBullet()
    {
        if (playerBullets.Count > 0)
        {
            GameObject obj = playerBullets.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return null;
    }

    public void ReturnBullet(GameObject uselessBullet)
    {
        uselessBullet.SetActive(false);
        playerBullets.Enqueue(uselessBullet);
    }
}
